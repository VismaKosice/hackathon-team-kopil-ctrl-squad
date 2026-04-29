using Microsoft.Extensions.Logging;

namespace PensionCalculationEngine.Domain.Mutations;

internal sealed partial class MutationDispatcher
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Information,
        Message = "Dispatch starting: mutation_count={MutationCount}")]
    private partial void LogProcessingStarted(int mutationCount);

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Debug,
        Message = "Mutation [{Index}] started: {DefinitionName} id={MutationId}")]
    private partial void LogMutationStarted(int index, string definitionName, Guid mutationId);

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Debug,
        Message = "Mutation [{Index}] completed: {DefinitionName} messages={MessageCount} patch_ops={PatchOpCount} duration_ms={ElapsedMs:F3}")]
    private partial void LogMutationCompleted(
        int index,
        string definitionName,
        int messageCount,
        int patchOpCount,
        double elapsedMs);

    [LoggerMessage(
        EventId = 4,
        Level = LogLevel.Warning,
        Message = "Mutation [{Index}] halted dispatch: {DefinitionName}")]
    private partial void LogProcessingHalted(int index, string definitionName);

    [LoggerMessage(
        EventId = 5,
        Level = LogLevel.Information,
        Message = "Dispatch completed: outcome={Outcome} processed={ProcessedCount} messages={MessageCount} duration_ms={DurationMs}")]
    private partial void LogProcessingCompleted(
        string outcome,
        int processedCount,
        int messageCount,
        long durationMs);

    [LoggerMessage(
        EventId = 6,
        Level = LogLevel.Warning,
        Message = "Mutation [{Index}] has no registered handler: {DefinitionName} id={MutationId}")]
    private partial void LogUnknownMutation(int index, string definitionName, Guid mutationId);
}
