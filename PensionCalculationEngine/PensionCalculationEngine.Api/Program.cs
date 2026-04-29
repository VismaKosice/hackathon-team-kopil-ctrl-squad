using System.Text.Json.Serialization;
using PensionCalculationEngine.Api.Endpoints;
using PensionCalculationEngine.Api.Middleware;
using PensionCalculationEngine.Domain.DependencyInjection;
using PensionCalculationEngine.Domain.Json;

namespace PensionCalculationEngine.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateSlimBuilder(args);

        var port = Environment.GetEnvironmentVariable("PORT");
        if (!string.IsNullOrWhiteSpace(port))
        {
            builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
        }
        else
        {
            builder.WebHost.UseUrls("http://0.0.0.0:8080");
        }

        builder.Services.ConfigureHttpJsonOptions(static options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, PensionJsonContextProvider.Default);
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        });

        builder.Services.RegisterDomainLayer();

        var app = builder.Build();

        app.UseMiddleware<RequestTimingMiddleware>();
        app.MapPost(CalculationEndpoint.Route, CalculationEndpoint.HandleAsync);

        app.Run();
    }
}
