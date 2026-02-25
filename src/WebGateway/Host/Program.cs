using Courses;
using Courses.Contracts;
using Courses.Features.Courses.CreateCourse;
using Courses.Features.Courses.GetCourseExistence;
using MassTransit;
using Media;
using Media.Features.Videos.UploadVideo;
using Media.Infrastructure.Data;
using Shared.Abstractions.Mediator;
using Shared.Infrastructure;
using Users;
using Users.Features.Enrollments;
using Identity;
using Identity.Features.Users.Register;
using Identity.Features.Users.Login;
using Shared.Infrastructure.Auth;
using Shared.Infrastructure.Validators;
using Shared.Infrastructure.Mediator.Behaviors;
using Shared.Infrastructure.Response;
using Shared.Abstractions.Response;
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
builder.Services.AddScoped<ICommandHandler<CreateCourseCommand, Result<Guid>>, CreateCourseHandler>();
builder.Services.AddScoped<IQueryHandler<GetCourseExistenceQuery, bool>, GetCourseExistenceHandler>();
builder.Services.AddScoped<ICommandHandler<RegisterCommand, Guid>, RegisterHandler>();
builder.Services.AddScoped<ICommandHandler<EnrollInCourseCommand, Guid>, EnrollInCourseHandler>();
builder.Services.AddScoped<ICommandHandler<LoginCommand, string>, LoginHandler>();

builder.Services.AddProjectValidators(typeof(CoursesModule).Assembly);
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CoursesModule).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddMediaModule(builder.Configuration);
builder.Services.AddCoursesModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
var app = builder.Build();
app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
var apiGroup = app.MapGroup(string.Empty)
    .AddEndpointFilter<ResultEndpointFilter>();
apiGroup.MapUploadVideo();
apiGroup.MapCreateCourse();
app.UseHttpsRedirection();
apiGroup.MapEnrollInCourse();
apiGroup.MapRegister();
apiGroup.MapLogin();
app.Run();


