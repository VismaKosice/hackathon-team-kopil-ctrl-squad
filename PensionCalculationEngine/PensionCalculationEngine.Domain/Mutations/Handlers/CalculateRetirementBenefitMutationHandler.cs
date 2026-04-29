using Microsoft.Extensions.Logging;
using PensionCalculationEngine.Domain.Common;
using PensionCalculationEngine.Domain.Mutations.Properties;
using PensionCalculationEngine.Domain.SchemeRegistry;
using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

/// <summary>
/// Handles <c>calculate_retirement_benefit</c> — computes weighted-average pension and per-policy
/// distribution; flips dossier to RETIRED. Honours per-scheme accrual rates from the
/// Scheme Registry when configured, otherwise uses the default 0.02.
/// </summary>
internal sealed partial class CalculateRetirementBenefitMutationHandler : IMutationHandler
{
    private readonly ISchemeRegistryClient _schemeRegistry;
    private readonly ILogger<CalculateRetirementBenefitMutationHandler> _logger;

    public CalculateRetirementBenefitMutationHandler(
        ISchemeRegistryClient schemeRegistry,
        ILogger<CalculateRetirementBenefitMutationHandler> logger)
    {
        _schemeRegistry = schemeRegistry;
        _logger = logger;
    }

    public string MutationDefinitionName => Constants.MutationDefinitions.CalculateRetirementBenefit;

    public async ValueTask HandleAsync(
        MutationBaseRequest mutation,
        CalculationContext context,
        CancellationToken cancellationToken = default)
    {
        if (context.Dossier is null)
        {
            context.AddCritical(Constants.MessageCode.DossierNotFound);
            return;
        }

        var policies = context.Dossier.Policies;
        if (policies.Count == 0)
        {
            context.AddCritical(Constants.MessageCode.NoPolicies);
            return;
        }

        var props = CalculateRetirementBenefitProperties.From(mutation.MutationProperties);
        var retirementDayNumber = props.RetirementDate.DayNumber;
        var policyCount = policies.Count;

        var years = new double[policyCount];
        var totalYears = 0.0;

        for (var i = 0; i < policyCount; i++)
        {
            var policy = policies[i];
            var diffDays = retirementDayNumber - policy.EmploymentStartDate.DayNumber;

            double policyYears;
            if (diffDays <= 0)
            {
                context.AddWarning(Constants.MessageCode.RetirementBeforeEmployment);
                policyYears = 0.0;
            }
            else
            {
                policyYears = diffDays / Constants.Calculation.DaysPerYear;
            }

            years[i] = policyYears;
            totalYears += policyYears;
        }

        var person = context.Dossier.Persons.Count > 0
            ? context.Dossier.Persons[0]
            : null;

        var ageYears = person is null
            ? 0.0
            : (retirementDayNumber - person.BirthDate.DayNumber) / Constants.Calculation.DaysPerYear;

        if (ageYears < Constants.Calculation.RetirementAgeYears
            && totalYears < Constants.Calculation.FullCareerYears)
        {
            context.AddCritical(Constants.MessageCode.NotEligible);
            LogNotEligible(ageYears, totalYears);
            return;
        }

        var uniqueSchemes = new HashSet<string>(policyCount, StringComparer.Ordinal);
        for (var i = 0; i < policyCount; i++)
        {
            uniqueSchemes.Add(policies[i].SchemeId);
        }

        var accruals = await _schemeRegistry
            .GetAccrualRatesAsync(uniqueSchemes, cancellationToken)
            .ConfigureAwait(false);

        var totalContribution = 0.0;
        for (var i = 0; i < policyCount; i++)
        {
            var policy = policies[i];
            var accrual = accruals.TryGetValue(policy.SchemeId, out var rate)
                ? rate
                : Constants.Calculation.DefaultAccrualRate;

            totalContribution += (double)(policy.Salary * policy.PartTimeFactor)
                                 * years[i]
                                 * accrual;
        }

        for (var i = 0; i < policyCount; i++)
        {
            var share = totalYears > 0.0
                ? totalContribution * (years[i] / totalYears)
                : 0.0;

            policies[i].AttainablePension = (decimal)share;
        }

        context.Dossier.Status = Constants.DossierStatus.Retired;
        context.Dossier.RetirementDate = props.RetirementDate;

        LogRetirementCalculated(props.RetirementDate, totalYears, totalContribution, policyCount);
    }
}
