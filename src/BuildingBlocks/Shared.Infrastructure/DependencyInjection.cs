using System.Reflection;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Shared.Abstractions;
using Shared.Abstractions.Messaging.Integration;
using Shared.Abstractions.Messaging.Internal;
using Shared.Infrastructure.Messaging.Integration;
using Shared.Infrastructure.Messaging.Internal;

namespace Shared.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(
    this IServiceCollection services,
    IConfiguration configuration,
    Action<IBusRegistrationConfigurator>? busConfigurator = null, // Outbox vb. için kanca
    params Assembly[] moduleAssemblies)
    {
        services.AddHostedService<OutboxBackgroundService>();
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
        //mediatr
        services.AddScoped<IInternalBus, MediatRInternalCommandBus>();
        services.AddTransient(typeof(IRequestHandler<,>), typeof(MediatRCommandHandlerBridge<,>));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddScoped<IIntegrationEventBus, MassTransitEventBus>();
        services.AddMassTransit(x =>
 {
     foreach (var assembly in moduleAssemblies)
     {
         var consumerTypes = assembly.GetTypes()
             .Where(t => t.IsClass && !t.IsAbstract &&
                         t.GetInterfaces().Any(i => i.IsGenericType &&
                                                  i.GetGenericTypeDefinition() == typeof(IIntegrationConsumer<>)));

         foreach (var consumerType in consumerTypes)
         {
             var interfaceType = consumerType.GetInterfaces()
                 .FirstOrDefault(i => i.IsGenericType &&
                                      i.GetGenericTypeDefinition() == typeof(IIntegrationConsumer<>));

             if (interfaceType == null) continue;

             var eventType = interfaceType.GetGenericArguments()[0];

             var wrapperType = typeof(MassTransitConsumerWrapper<,>).MakeGenericType(eventType, consumerType);

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
