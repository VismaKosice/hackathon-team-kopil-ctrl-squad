using PensionCalculationEngine.Shared.Models;

namespace PensionCalculationEngine.Domain.Mutations;

/// <summary>Public entry point that runs a calculation request and returns the assembled response.</summary>
public interface IMutationDispatcher
{
    ValueTask<CalculationResponse> ProcessAsync(
        CalculationRequest request,
        CancellationToken cancellationToken = default);
}
