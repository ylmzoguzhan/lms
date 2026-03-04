namespace Shared.Abstractions.Request;

public interface IInternalEvent { }

public interface IInternalEventHandler<in TEvent>
    where TEvent : IInternalEvent
{
    Task HandleAsync(TEvent @event, CancellationToken ct = default);
}