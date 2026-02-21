using MassTransit;
using Media;
using Media.Features.Videos.ProcessVideoCallback;
using Media.Features.Videos.UploadVideo;
using Media.Infrastructure.Data;
using Minio;
using Shared.Abstractions;
using Shared.Contracts.Media;
using Shared.Infrastructure;
using Shared.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddSingleton<IMinioClient>(sp =>
{
    return new MinioClient()
        .WithEndpoint("localhost:9000")
        .WithCredentials("admin", "admin1234")
        .WithSSL(false)
        .Build();
});
builder.Services.AddScoped<VideoProcessedHandler>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<IntegrationEventWrapper<VideoProcessedIntegrationEvent, VideoProcessedHandler>>();
    x.AddEntityFrameworkOutbox<MediaDbContext>(o =>
    {
        o.UsePostgres();
        o.UseBusOutbox();
    });
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/");
        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(MediaModule).Assembly);
    });
builder.Services.AddMediaModule(builder.Configuration);
builder.Services.AddScoped<IStorageService, MinioStorageService>();
builder.Services.AddScoped<IIntegrationBus, MassTransitEventBus>();
var app = builder.Build();
app.MapUploadVideo();



app.UseHttpsRedirection();




app.Run();


