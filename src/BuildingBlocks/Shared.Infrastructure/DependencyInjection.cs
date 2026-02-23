using System.Reflection;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Shared.Abstractions;
using Shared.Abstractions.Messaging;
using Shared.Abstractions.Messaging.Internal;
using Shared.Infrastructure.Messaging.Integration;
using Shared.Infrastructure.Messaging.Internal;

namespace Shared.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? registerConsumers = null)
    {
        //MinioClient
        services.AddScoped<IStorageService, MinioStorageService>();
        services.AddSingleton<IMinioClient>(sp =>
        {
            return new MinioClient()
                .WithEndpoint("localhost:9000")
                .WithCredentials("admin", "admin1234")
                .WithSSL(false)
                .Build();
        });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddScoped<IInternalEventBus, MediatRInternalCommandBus>();

        services.AddTransient(typeof(IRequestHandler<,>), typeof(MediatRCommandHandlerBridge<,>));
        return services;
    }
    public static IServiceCollection AddSharedInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration,
    params Assembly[] moduleAssemblies)
    {
        services.AddMassTransit(x =>
        {
            foreach (var assembly in moduleAssemblies)
            {
                var handlerTypes = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationConsumer<>)));

                foreach (var handlerType in handlerTypes)
                {
                    var interfaceType = handlerType.GetInterfaces().First(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationConsumer<>));
                    var eventType = interfaceType.GetGenericArguments()[0];

                    var wrapperType = typeof(MassTransitConsumerWrapper<,>).MakeGenericType(eventType, handlerType);

                    x.AddConsumer(wrapperType);
                }
            }

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:Host"] ?? "localhost", "/");
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
