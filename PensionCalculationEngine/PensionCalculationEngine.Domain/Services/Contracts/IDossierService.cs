using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Domain.Services.Contracts;

public interface IDossierService
{
    void CreateDossier(IEnumerable<MutationBaseRequest> mutationGroups);
}