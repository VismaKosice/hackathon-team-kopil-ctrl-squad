using PensionCalculationEngine.Domain.Mutations;
using PensionCalculationEngine.Shared.Models;

namespace PensionCalculationEngine.Api.Endpoints;

/// <summary>
/// Minimal API endpoint for <c>POST /calculation-requests</c>. Replaces the previous MVC
/// controller so the request pipeline is fully AOT-compatible (the
/// <c>Microsoft.AspNetCore.Http.RequestDelegateGenerator</c> source-generates the binding
/// shim from this static method).
/// </summary>
internal static partial class CalculationEndpoint
{
    public const string Route = "/calculation-requests";

    public static async Task<IResult> HandleAsync(
        CalculationRequest request,
        IMutationDispatcher dispatcher,
        ILogger<CalculationDispatchMarker> logger,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return Results.BadRequest();
        }

        LogRequestReceived(
            logger,
            request.TenantId,
            request.CalculationInstructions.Mutations.Count);

        var response = await dispatcher.ProcessAsync(request, cancellationToken);

        LogRequestCompleted(
            logger,
            response.CalculationMetadata.CalculationId,
            response.CalculationMetadata.CalculationOutcome,
            response.CalculationMetadata.CalculationDurationMs);

        return Results.Ok(response);
    }
}

/// <summary>Marker type used purely as the <see cref="ILogger{TCategoryName}"/> category for the calculation endpoint.</summary>
internal sealed class CalculationDispatchMarker
{
}
