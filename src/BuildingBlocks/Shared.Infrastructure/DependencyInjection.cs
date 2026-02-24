using System.Reflection;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Shared.Abstractions;
using Shared.Abstractions.Messaging;
using Shared.Abstractions.Mediator;
using Shared.Infrastructure.Mediator;
using Shared.Infrastructure.Mediator.Bridges;
using Shared.Infrastructure.Mediator.Wrapper;
using Shared.Infrastructure.Messaging.MassTransit;


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
        services.AddScoped<IInternalBus, InternalBus>();
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
         var eventHandlerTypes = assembly.GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract &&
                    t.GetInterfaces().Any(i => i.IsGenericType &&
                                             i.GetGenericTypeDefinition() == typeof(IInternalEventHandler<>)));

         foreach (var handlerType in eventHandlerTypes)
         {
             var interfaceType = handlerType.GetInterfaces()
                 .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInternalEventHandler<>));

             services.AddScoped(interfaceType, handlerType);

             var eventType = interfaceType.GetGenericArguments()[0];
             var wrapperType = typeof(MediatRNotificationWrapper<>).MakeGenericType(eventType);
             var bridgeType = typeof(MediatRNotificationHandlerBridge<>).MakeGenericType(eventType);
             var notificationHandlerInterface = typeof(INotificationHandler<>).MakeGenericType(wrapperType);

             services.AddScoped(notificationHandlerInterface, bridgeType);
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
