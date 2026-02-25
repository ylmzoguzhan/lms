using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Mediator;
using Shared.Abstractions.Validator;
using Shared.Infrastructure.Mediator.Wrapper;

namespace Shared.Infrastructure.Mediator.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IServiceProvider sp)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : MediatRCommandWrapper<TResponse>
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var command = request.Command;

        if (command is not ICommand<TResponse>)
            return await next();
        var commandType = command.GetType();

        var validatorType = typeof(IAppValidator<>).MakeGenericType(commandType);

        var validators = sp.GetServices(validatorType).Cast<dynamic>();

        var allErrors = new List<ValidationError>();

        foreach (var validator in validators)
        {
            var errors = await validator.ValidateAsync((dynamic)command, ct);
            allErrors.AddRange(errors);
        }

        if (allErrors.Count != 0)
            throw new AppValidationException(allErrors);

        return await next();
    }
}