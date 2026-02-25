using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Validator;

namespace Shared.Infrastructure.Validators;

public static class ValidatorsDependency
{
    public static IServiceCollection AddProjectValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatorTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAppValidator<>)));

        foreach (var validatorType in validatorTypes)
        {
            var interfaceType = validatorType.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IAppValidator<>));
            services.AddScoped(interfaceType, validatorType);
        }

        return services;
    }
}
