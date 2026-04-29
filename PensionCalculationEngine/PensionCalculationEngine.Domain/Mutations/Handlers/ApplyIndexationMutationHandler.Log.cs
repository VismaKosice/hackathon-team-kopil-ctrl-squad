using Microsoft.Extensions.Logging;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

internal sealed partial class ApplyIndexationMutationHandler
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Indexation applied: percentage={Percentage} matched={MatchedCount}/{TotalCount}")]
    private partial void LogIndexationApplied(decimal percentage, int matchedCount, int totalCount);
}
