namespace Users.Features.Users.CreateUser;

public static class CreateUserEndpoint
{
    public static void MapCreateUser(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/createuser", async (CreateUserCommand command, [FromServices] IInternalBus internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return Results.Ok(result);
        }).WithTags("Users");
    }
}
