using PensionCalculationEngine.Domain.Extensions;
using PensionCalculationEngine.Domain.Model.DossierCreation;
using PensionCalculationEngine.Domain.Services.Contracts;
using PensionCalculationEngine.Shared.Models;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

namespace PensionCalculationEngine.Domain.Services;

public class CreateDossierService : ICreateDossierService
{
    private readonly CalculationResponse calculationResponse;
    private DossierResponse Dossier  => calculationResponse.CalculationResult.EndSituation.Situation.Dossier; 

    public CreateDossierService(CalculationResponse calculationResponse)
    {
        this.calculationResponse = calculationResponse;
    }

    
    public void CreatePersons(IReadOnlyList<MutationBaseRequest>? dossierCreationRequest)
    {
        ArgumentNullException.ThrowIfNull(dossierCreationRequest);
        
        Dossier.Persons ??= new List<PersonResponse>(dossierCreationRequest.Count());
        calculationResponse.CalculationResult.Mutations ??= new List<MutationDoneResponse>(dossierCreationRequest.Count());
        
        foreach (var mutation in dossierCreationRequest)
        {
            Dossier.Persons.Add(new PersonResponse()
            {
                Id = mutation.MutationProperties.GetGuidValue("person_id"),
                Role = "PARTICIPANT",
                Name = mutation.MutationProperties.GetStringValue("name"),
                BirthDate = mutation.MutationProperties.GetDateOnlyValue("birth_date")
            });
            
            calculationResponse.CalculationResult.Mutations.Add(new() 
            {
                Mutation = new CalculationMutationResponse
                {
                    MutationId = mutation.MutationId,
                    MutationDefinitionName = mutation.MutationDefinitionName,
                    MutationType = mutation.MutationType,
                    ActualAt = mutation.ActualAt,
                    MutationProperties = mutation.MutationProperties,
                    DossierId = mutation.DossierId
                },
                CalculationMessageIndexes = new List<int>()
            });
        }
    }
}