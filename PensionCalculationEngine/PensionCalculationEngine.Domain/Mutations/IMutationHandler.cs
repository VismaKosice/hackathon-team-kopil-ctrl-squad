using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Domain.Mutations;

/// <summary>
/// Contract for a single mutation type. Each handler validates its inputs (producing
/// CRITICAL/WARNING messages on the context) and applies the calculation logic to the
/// working dossier. Adding a new mutation = implement + register.
/// </summary>
internal interface IMutationHandler
{
    /// <summary>The <c>mutation_definition_name</c> this handler is registered for.</summary>
    string MutationDefinitionName { get; }

    /// <summary>Validate then (if not halted) apply the mutation against the context.</summary>
    ValueTask HandleAsync(
        MutationBaseRequest mutation,
        CalculationContext context,
        CancellationToken cancellationToken = default);
}
