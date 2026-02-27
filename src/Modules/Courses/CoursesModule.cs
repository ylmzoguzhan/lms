using Courses.Features.Courses.CreateCourse;
using Courses.Features.Courses.Dto;
using Courses.Features.Courses.GetCourseExistence;
using Courses.Features.Courses.GetCourses;
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

        return services;

    }
}
