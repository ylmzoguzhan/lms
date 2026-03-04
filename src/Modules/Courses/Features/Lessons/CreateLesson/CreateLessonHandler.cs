using Courses.Features.LessonMedia.CreateLessonMedia;
using Media.Contracts;
using Shared.Abstractions.Response;

namespace Courses.Features.Lessons.CreateLesson;

public record CreateLessonCommand(string Title, LessonType LessonType, Guid ModuleId, string FileName) : ICommand<Result<CreateLessonResponse>>;
public record CreateLessonResponse(Guid Id, string Url);
public class CreateLessonHandler(CoursesDbContext dbContext, IDispatcher dispatcher) : ICommandHandler<CreateLessonCommand, Result<CreateLessonResponse>>
{
    public async Task<Result<CreateLessonResponse>> HandleAsync(CreateLessonCommand command, CancellationToken ct = default)
    {
        var videoUploadResponse = await dispatcher.SendAsync(new UploadVideoCommand(command.FileName, "mp4"));
        var lesson = new Lesson(command.Title, command.LessonType, command.ModuleId);
        lesson.AddMedia(videoUploadResponse.VideoId, command.FileName);
        await dbContext.AddAsync(lesson, ct);
        var outbox = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(ProcessVideoIntegrationEvent).AssemblyQualifiedName,
            Content = JsonSerializer.Serialize(new ProcessVideoIntegrationEvent(videoUploadResponse.VideoId)),
            CreatedAt = DateTime.UtcNow
        };
        dbContext.OutboxMessages.Add(outbox);
        await dbContext.SaveChangesAsync(ct);
        return new Result<CreateLessonResponse>(new CreateLessonResponse(lesson.Id, videoUploadResponse.UploadUrl), true, null, 201);
    }
}
