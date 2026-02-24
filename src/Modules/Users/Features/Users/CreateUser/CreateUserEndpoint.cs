using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.Messaging.Internal;

namespace Users.Features.Users.CreateUser;

public static class CreateUserEndpoint
{
    public static void MapCreateUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/createuser", async (CreateUserCommand command, [FromServices] IInternalEventBus internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return Results.Ok(result);
        }).WithTags("Users");
    }
}
