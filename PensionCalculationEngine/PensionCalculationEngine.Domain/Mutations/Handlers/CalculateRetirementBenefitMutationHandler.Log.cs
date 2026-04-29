using Microsoft.Extensions.Logging;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

internal sealed partial class CalculateRetirementBenefitMutationHandler
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Retirement calculated: retirement_date={RetirementDate} total_years={TotalYears:F4} annual_pension={AnnualPension:F2} policies={PolicyCount}")]
    private partial void LogRetirementCalculated(
        DateOnly retirementDate,
        double totalYears,
        double annualPension,
        int policyCount);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Retirement rejected as ineligible: age_years={AgeYears:F4} total_years={TotalYears:F4}")]
    private partial void LogNotEligible(double ageYears, double totalYears);
}
