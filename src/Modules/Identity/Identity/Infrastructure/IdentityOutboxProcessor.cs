using System.Text.Json;
using Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Abstractions.Mediator;
using Shared.Abstractions.Messaging.Outbox;

namespace Identity.Infrastructure;

public class IdentityOutboxProcessor(IdentityDbContext dbContext, IInternalBus bus, ILogger<IdentityOutboxProcessor> logger) : IOutboxProcessor
{
    public async Task ProcessAsync(CancellationToken ct = default)
    {
        var messages = await dbContext.OutboxMessages
           .Where(x => x.ProcessedAt == null)
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
