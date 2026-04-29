using Microsoft.Extensions.Logging;
using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Domain.Mutations.Properties;
using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

/// <summary>Handles <c>apply_indexation</c> — multiplies salary by (1 + percentage) on matching policies.</summary>
internal sealed partial class ApplyIndexationMutationHandler : IMutationHandler
{
    private readonly ILogger<ApplyIndexationMutationHandler> _logger;

    public ApplyIndexationMutationHandler(ILogger<ApplyIndexationMutationHandler> logger)
    {
        _logger = logger;
    }

    public string MutationDefinitionName => Constants.MutationDefinitions.ApplyIndexation;

    public ValueTask HandleAsync(
        MutationBaseRequest mutation,
        CalculationContext context,
        CancellationToken cancellationToken = default)
    {
        if (context.Dossier is null)
        {
            context.AddCritical(Constants.MessageCode.DossierNotFound);
            return ValueTask.CompletedTask;
        }

        var policies = context.Dossier.Policies;
        if (policies.Count == 0)
        {
            context.AddCritical(Constants.MessageCode.NoPolicies);
            return ValueTask.CompletedTask;
        }

        var props = ApplyIndexationProperties.From(mutation.MutationProperties);
        var hasFilters = props.SchemeId is not null || props.EffectiveBefore is not null;
        var multiplier = 1m + props.Percentage;

        var matchedCount = 0;
        var clamped = false;

        for (var i = 0; i < policies.Count; i++)
        {
            var policy = policies[i];

            if (props.SchemeId is not null && policy.SchemeId != props.SchemeId)
            {
                continue;
            }

            if (props.EffectiveBefore is not null && policy.EmploymentStartDate >= props.EffectiveBefore.Value)
            {
                continue;
            }

            matchedCount++;
            var newSalary = policy.Salary * multiplier;
            if (newSalary < 0m)
            {
                newSalary = 0m;
                clamped = true;
            }

            policy.Salary = newSalary;
        }

        if (matchedCount == 0 && hasFilters)
        {
            context.AddWarning(Constants.MessageCode.NoMatchingPolicies);
        }

        if (clamped)
        {
            context.AddWarning(Constants.MessageCode.NegativeSalaryClamped);
        }

        LogIndexationApplied(props.Percentage, matchedCount, policies.Count);
        return ValueTask.CompletedTask;
    }
}
