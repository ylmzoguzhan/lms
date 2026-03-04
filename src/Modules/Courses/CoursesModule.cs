using Courses.Features.Courses.CreateCourse;
using Courses.Features.Courses.DeleteCourse;
using Courses.Features.Courses.Dto;
using Courses.Features.Courses.GetCourseExistence;
using Courses.Features.Courses.GetCourses;
using Courses.Features.LessonMedia.CreateLessonMedia;
using Courses.Features.Lessons.CreateLesson;
using Courses.Features.Modules.CreateModule;
using Courses.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Response;


namespace Courses;

public static class CoursesModule
{
    public static IServiceCollection AddCoursesModule(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("CoursesDb");
        services.AddDbContext<CoursesDbContext>((sp, options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();

            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsHistoryTable("__EFMigrationsHistory", "courses");
            }).AddInterceptors(interceptors);
        });
        services.AddScoped<IOutboxProcessor, CoursesOutboxProcessor>();
        services.AddScoped<IQueryHandler<GetCoursesQuery, Result<PagedList<CourseDto>>>, GetCoursesHandler>();
        services.AddScoped<ICommandHandler<CreateCourseCommand, Result<Guid>>, CreateCourseHandler>();
        services.AddScoped<IQueryHandler<GetCourseExistenceQuery, bool>, GetCourseExistenceHandler>();
        services.AddScoped<ICommandHandler<DeleteCourseCommand, Result<bool>>, DeleteCourseHandler>();
        services.AddScoped<ICommandHandler<CreateModuleCommand, Result<Guid>>, CreateModuleHandler>();
        services.AddScoped<ICommandHandler<CreateLessonCommand, Result<CreateLessonResponse>>, CreateLessonHandler>();
        services.AddScoped<IInternalEventHandler<LessonMediaCreatedEvent>, LessonMediaCreatedHandler>();
        return services;

    }
}
