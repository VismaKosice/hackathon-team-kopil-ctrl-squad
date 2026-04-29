using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Messages;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

namespace PensionCalculationEngine.Domain.Mutations;

/// <summary>
/// Per-request mutable state shared between the dispatcher and mutation handlers.
/// Tracks the working dossier, message log, processed mutations and end-situation pointer.
/// </summary>
internal sealed class CalculationContext
{
    public DossierResponse? Dossier { get; set; }

    public List<CalculationMessageResponse> Messages { get; } = [];

    public List<MutationDoneResponse> ProcessedMutations { get; } = [];

    public List<int> CurrentMutationMessageIndexes { get; private set; } = [];

    public int NextPolicySequence { get; set; } = 1;

    public bool IsHalted { get; private set; }

    public Guid LastAppliedMutationId { get; set; }

    public int LastAppliedMutationIndex { get; set; }

    public DateOnly LastAppliedActualAt { get; set; }

    public void BeginMutation(List<int> messageIndexes)
    {
        CurrentMutationMessageIndexes = messageIndexes;
    }

    public void AddCritical(string code)
    {
        Messages.Add(new CalculationMessageResponse
        {
            Level = Constants.MessageLevel.Critical,
            Code = code,
        });
        CurrentMutationMessageIndexes.Add(Messages.Count - 1);
        IsHalted = true;
    }

    public void AddWarning(string code)
    {
        Messages.Add(new CalculationMessageResponse
        {
            Level = Constants.MessageLevel.Warning,
            Code = code,
        });
        CurrentMutationMessageIndexes.Add(Messages.Count - 1);
    }
}
