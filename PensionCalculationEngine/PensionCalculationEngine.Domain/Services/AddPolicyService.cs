using PensionCalculationEngine.Domain.Extensions;
using PensionCalculationEngine.Domain.Services.Contracts;
using PensionCalculationEngine.Shared.Models;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

namespace PensionCalculationEngine.Domain.Services;

public class AddPolicyService : IAddPolicyService
{
    private readonly CalculationResponse calculationResponse;
    private DossierResponse Dossier  => calculationResponse.CalculationResult.EndSituation.Situation.Dossier; 

    public AddPolicyService(CalculationResponse calculationResponse)
    {
        this.calculationResponse = calculationResponse;
    }

    public void AddPolicies(IReadOnlyList<MutationBaseRequest> dossierCreationRequest)
    {
        ArgumentNullException.ThrowIfNull(dossierCreationRequest);
        
        Dossier.Policies ??= new List<PolicyReponse>(dossierCreationRequest.Count());
        calculationResponse.CalculationResult.Mutations ??= new List<MutationDoneResponse>(dossierCreationRequest.Count());
        
        int indexPolicy = 1;
        foreach (var mutation in dossierCreationRequest)
        {
            Dossier.Policies.Add(new PolicyReponse()
            {
                PolicyId = $"{mutation.DossierId!.Value}-{indexPolicy++}",
                SchemeId = mutation.MutationProperties.GetStringValue("scheme_id"),
                EmploymentStartDate = mutation.MutationProperties.GetDateOnlyValue("employment_start_date"),
                Salary = mutation.MutationProperties.GetIntValue("salary"),
                PartTimeFactor = mutation.MutationProperties.GetDecimalValue("part_time_factor"),
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