using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Domain.Services.Contracts;

public interface IAddIndexationService
{
    void ApplyIndexation(IReadOnlyList<MutationBaseRequest> dossierCreationRequest);
}