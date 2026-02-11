using PensionCalculationEngine.Domain.Extensions;
using PensionCalculationEngine.Domain.Services.Contracts;
using PensionCalculationEngine.Shared.Models;
using PensionCalculationEngine.Shared.Models.PartialRequests;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.EndSituation;
using PensionCalculationEngine.Shared.Models.PartialResponses.CalculationResult.Mutations;

namespace PensionCalculationEngine.Domain.Services;

public class AddIndexationService : IAddIndexationService
{
    private readonly CalculationResponse calculationResponse;
    private DossierResponse Dossier  => calculationResponse.CalculationResult.EndSituation.Situation.Dossier; 
    private EndSituationReponse EndSituation  => calculationResponse.CalculationResult.EndSituation;
    
    public AddIndexationService(CalculationResponse calculationResponse)
    {
        this.calculationResponse = calculationResponse;
    }

    public void ApplyIndexation(IReadOnlyList<MutationBaseRequest> dossierCreationRequest)
    {
        ArgumentNullException.ThrowIfNull(dossierCreationRequest);
        
        calculationResponse.CalculationResult.Mutations ??= new List<MutationDoneResponse>(dossierCreationRequest.Count());
        
        int indexPolicy = 1;
        foreach (var mutation in dossierCreationRequest)
        {
            if (EndSituation.ActualAt is null 
                || EndSituation.ActualAt != mutation.ActualAt)
            {
                EndSituation.ActualAt = mutation.ActualAt;
            }
            
            var percentage = mutation.MutationProperties.GetDecimalValue("percentage");

            var policies = calculationResponse.CalculationResult.EndSituation.Situation.Dossier.Policies.AsEnumerable();
            
            try
            {
                var schemeId = mutation.MutationProperties.GetStringValue("scheme_id");
                policies = policies.Where(p => p.SchemeId == schemeId);
            }
            catch {}
            
            try
            {
                var effectiveBefore = mutation.MutationProperties.GetDateOnlyValue("effective_before");
                policies = policies.Where(p => p.EmploymentStartDate <= effectiveBefore);
            }
            catch {}

            foreach (var policy in policies)
            {
                policy.Salary = (int)(policy.Salary * (1 + percentage));
            }
            
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