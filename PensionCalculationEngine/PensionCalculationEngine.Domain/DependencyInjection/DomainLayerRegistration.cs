using Microsoft.Extensions.DependencyInjection;
using PensionCalculationEngine.Domain.Mutations;
using PensionCalculationEngine.Domain.Mutations.Handlers;
using PensionCalculationEngine.Domain.SchemeRegistry;

namespace PensionCalculationEngine.Domain.DependencyInjection;

public static class DomainLayerRegistration
{
    public static IServiceCollection RegisterDomainLayer(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(SchemeRegistryClient));
        services.AddSingleton<ISchemeRegistryClient, SchemeRegistryClient>();

        services.AddSingleton<IMutationHandler, CreateDossierMutationHandler>();
        services.AddSingleton<IMutationHandler, AddPolicyMutationHandler>();
        services.AddSingleton<IMutationHandler, ApplyIndexationMutationHandler>();
        services.AddSingleton<IMutationHandler, CalculateRetirementBenefitMutationHandler>();
        services.AddSingleton<IMutationHandler, ProjectFutureBenefitsMutationHandler>();

        services.AddSingleton<IMutationDispatcher, MutationDispatcher>();

        return services;
    }
}
