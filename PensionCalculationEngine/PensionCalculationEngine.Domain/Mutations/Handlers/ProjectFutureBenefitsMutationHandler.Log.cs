using Microsoft.Extensions.Logging;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

internal sealed partial class ProjectFutureBenefitsMutationHandler
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Projections generated: points={ProjectionPoints} policies={PolicyCount}")]
    private partial void LogProjectionsGenerated(int projectionPoints, int policyCount);
}
