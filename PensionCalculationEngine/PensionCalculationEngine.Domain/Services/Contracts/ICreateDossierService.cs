using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

namespace PensionCalculationEngine.Domain.Services.Contracts;

public interface ICreateDossierService
{
    void CreatePersons(IReadOnlyList<MutationBaseRequest> dossierCreationRequest);
}