using Shared.Abstractions.Validator;

namespace Courses.Features.Courses.CreateCourse;

public class CreateCourseValidator : IAppValidator<CreateCourseCommand>
{
    public Task<IEnumerable<ValidationError>> ValidateAsync(CreateCourseCommand request, CancellationToken ct)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrEmpty(request.title))
            errors.Add(new ValidationError("Course.Title", "Başlık boş olamaz."));

        return Task.FromResult(errors.AsEnumerable());
    }
}