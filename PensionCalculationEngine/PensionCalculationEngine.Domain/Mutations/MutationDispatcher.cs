using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Domain.Json;
using PensionCalculationEngine.Domain.JsonPatch;
using PensionCalculationEngine.Shared.Models;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationMetadata;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.InitialSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

namespace PensionCalculationEngine.Domain.Mutations;

/// <summary>
/// Sequentially processes the request's mutations through registered <see cref="IMutationHandler"/>s,
/// generates forward/backward JSON Patches per mutation, and assembles the <see cref="CalculationResponse"/>.
/// </summary>
internal sealed partial class MutationDispatcher : IMutationDispatcher
{
    private readonly Dictionary<string, IMutationHandler> _handlers;
    private readonly ILogger<MutationDispatcher> _logger;

    public MutationDispatcher(
        IEnumerable<IMutationHandler> handlers,
        ILogger<MutationDispatcher> logger)
    {
        _handlers = handlers.ToDictionary(static h => h.MutationDefinitionName, StringComparer.Ordinal);
        _logger = logger;
    }

    public async ValueTask<CalculationResponse> ProcessAsync(
        CalculationRequest request,
        CancellationToken cancellationToken = default)
    {
        var startedAt = DateTime.UtcNow;
        var mutations = request.CalculationInstructions.Mutations;
        var context = new CalculationContext();

        LogProcessingStarted(mutations.Count);

        var initialActualAt = mutations.Count > 0
            ? mutations[0].ActualAt
            : default;

        if (mutations.Count > 0)
        {
            context.LastAppliedMutationId = mutations[0].MutationId;
            context.LastAppliedMutationIndex = 0;
            context.LastAppliedActualAt = initialActualAt;
        }

        var previousSnapshot = SerializeSituation(dossier: null);

        for (var i = 0; i < mutations.Count; i++)
        {
            var mutation = mutations[i];
            var messageIndexes = new List<int>();
            context.BeginMutation(messageIndexes);

            var handlerStart = Stopwatch.GetTimestamp();
            if (_handlers.TryGetValue(mutation.MutationDefinitionName, out var handler))
            {
                LogMutationStarted(i, mutation.MutationDefinitionName, mutation.MutationId);
                await handler.HandleAsync(mutation, context, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                LogUnknownMutation(i, mutation.MutationDefinitionName, mutation.MutationId);
            }

            var elapsedMs = Stopwatch.GetElapsedTime(handlerStart).TotalMilliseconds;

            var currentSnapshot = SerializeSituation(context.Dossier);
            var forwardPatch = JsonPatchDiff.Diff(previousSnapshot, currentSnapshot);
            var backwardPatch = JsonPatchDiff.Diff(currentSnapshot, previousSnapshot);

            context.ProcessedMutations.Add(new MutationDoneResponse
            {
                Mutation = new CalculationMutationResponse
                {
                    MutationId = mutation.MutationId,
                    MutationDefinitionName = mutation.MutationDefinitionName,
                    MutationType = mutation.MutationType,
                    ActualAt = mutation.ActualAt,
                    DossierId = mutation.DossierId,
                    MutationProperties = mutation.MutationProperties,
                },
                CalculationMessageIndexes = messageIndexes,
                ForwardPatchToSituationAfterThisMutation = forwardPatch,
                BackwardPatchToPreviousSituation = backwardPatch,
            });

            LogMutationCompleted(
                i,
                mutation.MutationDefinitionName,
                messageIndexes.Count,
                forwardPatch.Count,
                elapsedMs);

            if (context.IsHalted)
            {
                LogProcessingHalted(i, mutation.MutationDefinitionName);
                break;
            }

            previousSnapshot = currentSnapshot;
            context.LastAppliedMutationId = mutation.MutationId;
            context.LastAppliedMutationIndex = i;
            context.LastAppliedActualAt = mutation.ActualAt;
        }

        var completedAt = DateTime.UtcNow;
        var totalDurationMs = (long)(completedAt - startedAt).TotalMilliseconds;

        var outcome = context.IsHalted
            ? Constants.CalculationOutcome.Failure
            : Constants.CalculationOutcome.Success;

        LogProcessingCompleted(
            outcome,
            context.ProcessedMutations.Count,
            context.Messages.Count,
            totalDurationMs);

        return new CalculationResponse
        {
            CalculationMetadata = new CalculationMetadataResponse
            {
                CalculationId = Guid.NewGuid(),
                TenantId = request.TenantId,
                CalculationStartedAt = startedAt,
                CalculationCompletedAt = completedAt,
                CalculationDurationMs = totalDurationMs,
                CalculationOutcome = outcome,
            },
            CalculationResult = new CalculationResultResponse
            {
                Messages = context.Messages,
                InitialSituation = new InitialSituationResponse
                {
                    ActualAt = initialActualAt,
                    Situation = new SituationResponse { Dossier = null },
                },
                Mutations = context.ProcessedMutations,
                EndSituation = new EndSituationResponse
                {
                    MutationId = context.LastAppliedMutationId,
                    MutationIndex = context.LastAppliedMutationIndex,
                    ActualAt = context.LastAppliedActualAt,
                    Situation = new SituationResponse { Dossier = context.Dossier },
                },
            },
        };
    }

    private static JsonNode SerializeSituation(DossierResponse? dossier)
    {
        var situation = new SituationResponse { Dossier = dossier };
        return JsonSerializer.SerializeToNode(situation, PensionJsonContext.Default.SituationResponse)!;
    }
}
