using Courses;
using Courses.Contracts;
using Courses.Features.Courses.CreateCourse;
using Courses.Features.Courses.GetCourseExistence;
using MassTransit;
using Media;
using Media.Features.Videos.UploadVideo;
using Media.Infrastructure.Data;
using Shared.Abstractions.Messaging.Internal;
using Shared.Infrastructure;
using Users;
using Users.Features.Enrollments;
using Users.Features.Users.CreateUser;

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
    typeof(UsersModule).Assembly,
    typeof(MediaModule).Assembly
);
builder.Services.AddScoped<ICommandHandler<UploadVideoCommand, UploadVideoResponse>, UploadVideoHandler>();
builder.Services.AddScoped<ICommandHandler<CreateCourseCommand, Guid>, CreateCourseHandler>();
builder.Services.AddScoped<IQueryHandler<GetCourseExistenceQuery, bool>, GetCourseExistenceHandler>();
builder.Services.AddScoped<ICommandHandler<CreateUserCommand, Guid>, CreateUserHandler>();
builder.Services.AddScoped<ICommandHandler<EnrollInCourseCommand, Guid>, EnrollInCourseHandler>();

builder.Services.AddMediaModule(builder.Configuration);
builder.Services.AddCoursesModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
var app = builder.Build();
app.MapUploadVideo();
app.MapCreateCourse();
app.UseHttpsRedirection();
app.MapEnrollInCourse();
app.MapCreateUser();
app.Run();


