using Media.Features.Videos.ProcessVideoCallback;
using Media.Features.Videos.UploadVideo;
using Media.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Media;

public static class MediaModule
{
    public static IServiceCollection AddMediaModule(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MediaDb");
        services.AddDbContext<MediaDbContext>((sp, options) =>
        {
            var interceptors = sp.GetServices<ISaveChangesInterceptor>();
            options.UseNpgsql(connectionString, o =>
            {
                o.MigrationsHistoryTable("__EFMigrationsHistory", "media");
            }).AddInterceptors(interceptors);
        });
        services.AddScoped<VideoProcessedHandler>();
        services.AddScoped<UploadVideoHandler>();
        return services;

    }
}
