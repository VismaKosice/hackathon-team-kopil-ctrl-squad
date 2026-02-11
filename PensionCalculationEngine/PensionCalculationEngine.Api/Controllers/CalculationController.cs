using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using PensionCalculationEngine.Domain.Services.Contracts;
using PensionCalculationEngine.Shared.Models;
using PensionCalculationEngine.Shared.Models.PartialRequests;

namespace PensionCalculationEngine.Api.Controllers;

[ApiController]
[Route("calculation-requests")]
public class CalculationController : ControllerBase
{
    private readonly IDossierService _dossierService;
    private readonly ICreateDossierService _createDossierService;
    private readonly IAddPolicyService _addPolicyService;
    private readonly IAddIndexationService _addIndexationService;
    private readonly CalculationResponse calculationResponse;
    
    public CalculationController(
        IDossierService dossierService,
        ICreateDossierService createDossierService, 
        CalculationResponse calculationResponse, IAddPolicyService addPolicyService, IAddIndexationService addIndexationService)
    {
        _dossierService = dossierService;
        _createDossierService = createDossierService;
        this.calculationResponse = calculationResponse;
        _addPolicyService = addPolicyService;
        _addIndexationService = addIndexationService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CalculationRequest calculationRequest)
    {
        calculationResponse.CalculationMetadata = new();
        calculationResponse.CalculationMetadata.CalculationId = Guid.NewGuid();
        calculationResponse.CalculationMetadata.CalculationStartedAt = DateTime.UtcNow;
        calculationResponse.CalculationMetadata.CalculationOutcome = "SUCCESS";
            
        calculationResponse.CalculationResult = new();
        calculationResponse.CalculationResult.EndSituation = new();
        calculationResponse.CalculationResult.EndSituation.Situation = new();
        
        var mutationGroups = calculationRequest
            .CalculationInstructions
            .Mutations
            .GroupBy(m => m.MutationDefinitionName)
            .ToDictionary(m => m.Key, m => m.ToImmutableList());
        
        ImmutableList<MutationBaseRequest>? mutationGroupToProcess;

        mutationGroups.TryGetValue(Domain.Common.Constants.MutationDefinitions.CreateDossier,
            out mutationGroupToProcess);
        _dossierService.CreateDossier(mutationGroupToProcess ?? []);
        _createDossierService.CreatePersons(mutationGroupToProcess ?? []);

        mutationGroups.TryGetValue(Domain.Common.Constants.MutationDefinitions.AddPolicy, out mutationGroupToProcess);
        _addPolicyService.AddPolicies(mutationGroupToProcess ?? []);

        mutationGroups.TryGetValue(Domain.Common.Constants.MutationDefinitions.ApplyIndexation, out mutationGroupToProcess);
        _addIndexationService.ApplyIndexation(mutationGroupToProcess ?? []);

        calculationResponse.CalculationResult.EndSituation.MutationIndex = calculationResponse.CalculationResult.Mutations.Count - 1;
            
        calculationResponse.CalculationResult.EndSituation.Situation.Dossier.Status = "ACTIVE";
        calculationResponse.CalculationMetadata.CalculationCompletedAt = DateTime.UtcNow;
        calculationResponse.CalculationMetadata.CalculationDurationMs = 
            (long)(calculationResponse.CalculationMetadata.CalculationCompletedAt - calculationResponse.CalculationMetadata.CalculationStartedAt).TotalMilliseconds;
        
        return Ok(calculationResponse);
    }
}