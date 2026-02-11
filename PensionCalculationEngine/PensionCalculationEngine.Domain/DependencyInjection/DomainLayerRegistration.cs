using Microsoft.Extensions.DependencyInjection;
using PensionCalculationEngine.Domain.Services;
using PensionCalculationEngine.Domain.Services.Contracts;
using PensionCalculationEngine.Shared.Models;

namespace PensionCalculationEngine.Domain.DependencyInjection;

public static class DomainLayerRegistration
{
    public static void RegisterDomainLayer(this IServiceCollection services)
    {
        services.AddScoped<CalculationResponse>();
        
        services.AddScoped<IDossierService, DossierService>();
        services.AddScoped<ICreateDossierService, CreateDossierService>();
        services.AddScoped<IAddPolicyService, AddPolicyService>();
    }
    
}