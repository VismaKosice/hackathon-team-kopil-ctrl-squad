using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Domain.Services.Contracts;

public interface IAddPolicyService
{
    void AddPolicies(IReadOnlyList<MutationBaseRequest> dossierCreationRequest);
}