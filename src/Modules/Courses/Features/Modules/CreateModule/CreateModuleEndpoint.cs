namespace Courses.Features.Modules.CreateModule;

public static class CreateModuleEndpoint
{
    public static void MapCreateModule(this IEndpointRouteBuilder app)
    {
        app.MapPost("/courses/module", async (CreateModuleCommand command, [FromServices] IDispatcher internalBus) =>
        {
            var result = await internalBus.SendAsync(command);
            return result;
        }).WithTags("Modules").RequireAuthorization();
    }
}
