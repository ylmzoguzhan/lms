using Courses;
using Courses.Features.Courses.CreateCourse;
using MassTransit;
using Media;
using Media.Features.Videos.UploadVideo;
using Media.Infrastructure.Data;
using Shared.Abstractions.Messaging.Internal;
using Shared.Infrastructure;

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
    typeof(MediaModule).Assembly
);
builder.Services.AddScoped<ICommandHandler<UploadVideoCommand, UploadVideoResponse>, UploadVideoHandler>();
builder.Services.AddScoped<ICommandHandler<CreateCourseCommand, Guid>, CreateCourseHandler>();


builder.Services.AddMediaModule(builder.Configuration);
builder.Services.AddCoursesModule(builder.Configuration);
var app = builder.Build();
app.MapUploadVideo();
app.MapCreateCourse();
app.UseHttpsRedirection();
app.Run();


