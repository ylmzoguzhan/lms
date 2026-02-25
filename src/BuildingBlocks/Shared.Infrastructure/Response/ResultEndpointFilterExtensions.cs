using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Shared.Infrastructure.Response;

public static class ResultEndpointFilterExtensions
{
    public static IServiceCollection AddResultEndpointFilter(this IServiceCollection services)
    {
        services.TryAddSingleton<ResultEndpointFilter>();
        return services;
    }

    public static RouteHandlerBuilder WithResultFilter(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ResultEndpointFilter>();
    }

    public static RouteGroupBuilder WithResultFilter(this RouteGroupBuilder builder)
    {
        builder.AddEndpointFilter<ResultEndpointFilter>();
        return builder;
    }
}