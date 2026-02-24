using Courses;
using Courses.Features.Courses.CreateCourse;
using Courses.Features.Courses.GetCourseExistence;
using MassTransit;
using Media;
using Media.Features.Videos.UploadVideo;
using Media.Infrastructure.Data;
using Shared.Abstractions.Messaging.Internal;
using Shared.Infrastructure;
using Users;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

builder.Services.AddSharedInfrastructure(
    builder.Configuration,
    x =>
    {
        x.AddEntityFrameworkOutbox<MediaDbContext>(o =>
        {
            o.UsePostgres();
            o.UseBusOutbox();
        });
    },
    typeof(MediaModule).Assembly,
    typeof(CoursesModule).Assembly,
    typeof(UsersModule).Assembly
);
builder.Services.AddScoped<ICommandHandler<UploadVideoCommand, UploadVideoResponse>, UploadVideoHandler>();
builder.Services.AddScoped<ICommandHandler<CreateCourseCommand, Guid>, CreateCourseHandler>();
builder.Services.AddScoped<IQueryHandler<GetCourseExistenceQuery, bool>, GetCourseExistenceHandler>();

builder.Services.AddMediaModule(builder.Configuration);
builder.Services.AddCoursesModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
var app = builder.Build();
app.MapUploadVideo();
app.MapCreateCourse();
app.UseHttpsRedirection();
app.Run();


