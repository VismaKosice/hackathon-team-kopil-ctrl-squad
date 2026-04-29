using Microsoft.Extensions.Logging;
using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Domain.Mutations.Properties;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

/// <summary>Handles <c>add_policy</c> — appends a policy with auto-generated <c>{dossier_id}-{n}</c> id.</summary>
internal sealed partial class AddPolicyMutationHandler : IMutationHandler
{
    private readonly ILogger<AddPolicyMutationHandler> _logger;

    public AddPolicyMutationHandler(ILogger<AddPolicyMutationHandler> logger)
    {
        _logger = logger;
    }

    public string MutationDefinitionName => Constants.MutationDefinitions.AddPolicy;

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

        var props = AddPolicyProperties.From(mutation.MutationProperties);

        if (props.Salary < 0m)
        {
            context.AddCritical(Constants.MessageCode.InvalidSalary);
            return ValueTask.CompletedTask;
        }

        if (props.PartTimeFactor < 0m || props.PartTimeFactor > 1m)
        {
            context.AddCritical(Constants.MessageCode.InvalidPartTimeFactor);
            return ValueTask.CompletedTask;
        }

        var policies = context.Dossier.Policies;
        for (var i = 0; i < policies.Count; i++)
        {
            var existing = policies[i];
            if (existing.SchemeId == props.SchemeId
                && existing.EmploymentStartDate == props.EmploymentStartDate)
            {
                context.AddWarning(Constants.MessageCode.DuplicatePolicy);
                break;
            }
        }

        var sequence = context.NextPolicySequence++;
        var policyId = $"{context.Dossier.Id}-{sequence}";

        policies.Add(new PolicyResponse
        {
            PolicyId = policyId,
            SchemeId = props.SchemeId,
            EmploymentStartDate = props.EmploymentStartDate,
            Salary = props.Salary,
            PartTimeFactor = props.PartTimeFactor,
            AttainablePension = null,
            Projections = null,
        });

        LogPolicyAdded(policyId, props.SchemeId);
        return ValueTask.CompletedTask;
    }
}
