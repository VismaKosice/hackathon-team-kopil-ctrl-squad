using Microsoft.Extensions.Logging;
using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Domain.Mutations.Properties;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

/// <summary>
/// Handles <c>project_future_benefits</c> — produces a per-policy projection series using the
/// retirement formula at each projection date, skipping eligibility checks. Status unchanged.
/// </summary>
internal sealed partial class ProjectFutureBenefitsMutationHandler : IMutationHandler
{
    private readonly ILogger<ProjectFutureBenefitsMutationHandler> _logger;

    public ProjectFutureBenefitsMutationHandler(ILogger<ProjectFutureBenefitsMutationHandler> logger)
    {
        _logger = logger;
    }

    public string MutationDefinitionName => Constants.MutationDefinitions.ProjectFutureBenefits;

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

        var props = ProjectFutureBenefitsProperties.From(mutation.MutationProperties);
        if (props.ProjectionEndDate <= props.ProjectionStartDate || props.ProjectionIntervalMonths <= 0)
        {
            context.AddCritical(Constants.MessageCode.InvalidDateRange);
            return ValueTask.CompletedTask;
        }

        var policyCount = policies.Count;
        var beforeAnyEmployment = false;

        for (var i = 0; i < policyCount; i++)
        {
            if (props.ProjectionStartDate < policies[i].EmploymentStartDate)
            {
                beforeAnyEmployment = true;
                break;
            }
        }

        if (beforeAnyEmployment)
        {
            context.AddWarning(Constants.MessageCode.ProjectionBeforeEmployment);
        }

        for (var i = 0; i < policyCount; i++)
        {
            policies[i].Projections ??= [];
        }

        var years = new double[policyCount];
        var accrualRate = Constants.Calculation.DefaultAccrualRate;
        var projectionPoints = 0;

        for (var date = props.ProjectionStartDate;
             date <= props.ProjectionEndDate;
             date = date.AddMonths(props.ProjectionIntervalMonths))
        {
            var dateDayNumber = date.DayNumber;
            var totalYears = 0.0;
            var weightedSalarySum = 0.0;

            for (var i = 0; i < policyCount; i++)
            {
                var policy = policies[i];
                var diffDays = dateDayNumber - policy.EmploymentStartDate.DayNumber;
                var policyYears = diffDays > 0
                    ? diffDays / Constants.Calculation.DaysPerYear
                    : 0.0;

                years[i] = policyYears;
                totalYears += policyYears;
                weightedSalarySum += (double)(policy.Salary * policy.PartTimeFactor) * policyYears;
            }

            var annualPension = weightedSalarySum * accrualRate;

            for (var i = 0; i < policyCount; i++)
            {
                var share = totalYears > 0.0
                    ? annualPension * (years[i] / totalYears)
                    : 0.0;

                policies[i].Projections!.Add(new PolicyProjectionResponse
                {
                    Date = date,
                    ProjectedPension = (decimal)share,
                });
            }

            projectionPoints++;
        }

        LogProjectionsGenerated(projectionPoints, policyCount);
        return ValueTask.CompletedTask;
    }
}
