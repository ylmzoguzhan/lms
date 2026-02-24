using System.Text.Json;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Abstractions.Messaging.Internal;
using Shared.Abstractions.Messaging.Outbox;

namespace Courses.Infrastructure.Messaging;

public class CoursesOutboxProcessor(
    CoursesDbContext dbContext,
    IInternalBus bus,
    ILogger<CoursesOutboxProcessor> logger) : IOutboxProcessor
{
    public async Task ProcessAsync(CancellationToken ct = default)
    {
        var messages = await dbContext.OutboxMessages
            .OrderBy(x => x.CreatedAt)
            .Take(20)
            .ToListAsync(ct);

        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type);
                if (type == null) continue;

                var @event = JsonSerializer.Deserialize(message.Content, type);

                if (@event is IInternalEvent internalEvent)
                    await bus.PublishAsync((dynamic)internalEvent, ct);

                message.ProcessedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Mesaj işlenemedi: {MessageId}", message.Id);
            }
        }

        await dbContext.SaveChangesAsync(ct);
    }
}