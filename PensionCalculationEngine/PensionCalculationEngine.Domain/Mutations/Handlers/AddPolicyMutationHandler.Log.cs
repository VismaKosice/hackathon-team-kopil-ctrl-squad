using Microsoft.Extensions.Logging;

namespace PensionCalculationEngine.Domain.Mutations.Handlers;

internal sealed partial class AddPolicyMutationHandler
{
    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Debug,
        Message = "Policy added: policy_id={PolicyId} scheme_id={SchemeId}")]
    private partial void LogPolicyAdded(string policyId, string schemeId);
}
