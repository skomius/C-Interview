using C_API_Interview.IServices;

namespace C_API_Interview.Extensions;

public static class CoreScope
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<Orchestrator, OrchestratorImpl>();
        services.AddSingleton<IBlobStor, BlobStor>();
        services.AddSingleton<IJsonValidator, JsonProcessorImpl>();
        return services;
    }
}