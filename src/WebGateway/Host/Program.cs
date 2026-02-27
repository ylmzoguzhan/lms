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
using Courses.Features.Courses.GetCourses;
using Courses.Features.Courses.DeleteCourse;
using Shared.Infrastructure.GlobalExceptions;
using Shared.Infrastructure.Mediator;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

//Modül
builder.Services.AddMediaModule(builder.Configuration);
builder.Services.AddCoursesModule(builder.Configuration);
builder.Services.AddUsersModule(builder.Configuration);
builder.Services.AddIdentityModule(builder.Configuration);

//sayfalama
builder.Services.AddScoped<IPagedListFactory, EfPagedListFactory>();

//mediatr
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CoursesModule).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});


//validator
builder.Services.AddProjectValidators(typeof(CoursesModule).Assembly);

//exceptions
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
//token
builder.Services.AddJwtAuthentication(builder.Configuration);


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
builder.Services.AddScoped<IInternalBus, InternalBus>();
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
apiGroup.MapGetCourses();
apiGroup.MapDeleteCourse();
app.Run();


