namespace PensionCalculationEngine.Domain.Common;

internal static class Constants
{
    public static class MutationDefinitions
    {
        public const string CreateDossier = "create_dossier";
        public const string AddPolicy = "add_policy";
        public const string ApplyIndexation = "apply_indexation";
        public const string CalculateRetirementBenefit = "calculate_retirement_benefit";
        public const string ProjectFutureBenefits = "project_future_benefits";
    }

    public static class DossierStatus
    {
        public const string Active = "ACTIVE";
        public const string Retired = "RETIRED";
    }

    public static class PersonRole
    {
        public const string Participant = "PARTICIPANT";
    }

    public static class CalculationOutcome
    {
        public const string Success = "SUCCESS";
        public const string Failure = "FAILURE";
    }

    public static class MessageLevel
    {
        public const string Critical = "CRITICAL";
        public const string Warning = "WARNING";
    }

    public static class MessageCode
    {
        public const string DossierAlreadyExists = "DOSSIER_ALREADY_EXISTS";
        public const string DossierNotFound = "DOSSIER_NOT_FOUND";
        public const string InvalidBirthDate = "INVALID_BIRTH_DATE";
        public const string InvalidName = "INVALID_NAME";
        public const string InvalidSalary = "INVALID_SALARY";
        public const string InvalidPartTimeFactor = "INVALID_PART_TIME_FACTOR";
        public const string DuplicatePolicy = "DUPLICATE_POLICY";
        public const string NoPolicies = "NO_POLICIES";
        public const string NoMatchingPolicies = "NO_MATCHING_POLICIES";
        public const string NegativeSalaryClamped = "NEGATIVE_SALARY_CLAMPED";
        public const string NotEligible = "NOT_ELIGIBLE";
        public const string RetirementBeforeEmployment = "RETIREMENT_BEFORE_EMPLOYMENT";
        public const string InvalidDateRange = "INVALID_DATE_RANGE";
        public const string ProjectionBeforeEmployment = "PROJECTION_BEFORE_EMPLOYMENT";
    }

    public static class Calculation
    {
        public const double DaysPerYear = 365.25;
        public const double DefaultAccrualRate = 0.02;
        public const int RetirementAgeYears = 65;
        public const double FullCareerYears = 40.0;
    }
}
