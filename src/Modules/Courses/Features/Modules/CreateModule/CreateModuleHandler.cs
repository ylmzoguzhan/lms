using Shared.Abstractions.Response;

namespace Courses.Features.Modules.CreateModule;

public record CreateModuleCommand(string Title, string Description, Guid CourseId, string FileName) : ICommand<Result<Guid>>;
public class CreateModuleHandler(CoursesDbContext dbContext) : ICommandHandler<CreateModuleCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(CreateModuleCommand command, CancellationToken ct = default)
    {
        var module = new Module(command.Title, command.Description, command.CourseId);
        await dbContext.Modules.AddAsync(module, ct);
        await dbContext.SaveChangesAsync(ct);
        return new Result<Guid>(module.Id, true, null, 201);
    }
}
