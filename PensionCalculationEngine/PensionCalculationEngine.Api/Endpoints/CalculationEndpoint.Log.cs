namespace PensionCalculationEngine.Api.Endpoints;

internal static partial class CalculationEndpoint
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Calculation request received: tenant={TenantId} mutations={MutationCount}")]
    private static partial void LogRequestReceived(
        ILogger logger,
        string tenantId,
        int mutationCount);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Information,
        Message = "Calculation completed: id={CalculationId} outcome={Outcome} duration_ms={DurationMs}")]
    private static partial void LogRequestCompleted(
        ILogger logger,
        Guid calculationId,
        string outcome,
        long durationMs);
}
