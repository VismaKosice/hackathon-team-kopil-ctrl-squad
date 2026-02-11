using System.Collections.Immutable;
using PensionCalculationEngine.Domain.Extensions;
using PensionCalculationEngine.Domain.Services.Contracts;
using PensionCalculationEngine.Shared.Models;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;

namespace PensionCalculationEngine.Domain.Services;

public class DossierService : IDossierService
{
    private readonly CalculationResponse calculationResponse;
    private EndSituationReponse EndSituation  => calculationResponse.CalculationResult.EndSituation;
    private SituationReponse Situation  => calculationResponse.CalculationResult.EndSituation.Situation; 

    public DossierService(CalculationResponse calculationResponse)
    {
        this.calculationResponse = calculationResponse;
    }

    public void CreateDossier(IEnumerable<MutationBaseRequest> createDossier)
    {
        Situation.Dossier ??= new();

        foreach (var mutation in createDossier)
        {
            if (EndSituation.ActualAt is null 
                || EndSituation.ActualAt != mutation.ActualAt)
            {
                EndSituation.ActualAt = mutation.ActualAt;
            }
            
            if (mutation.MutationProperties.GetStringValue(Common.Constants.MutationPropertiesDefinitions.CreateDossierDefinitions.DossierId) is string dossierIdCurrent)
            {
                if (Situation.Dossier.Id is not null 
                    && Situation.Dossier.Id != dossierIdCurrent)
                {
                    throw new ArgumentException("Dossier id must be unique");
                }
                
                Situation.Dossier.Id = dossierIdCurrent;
                
            }
        }
    }
}