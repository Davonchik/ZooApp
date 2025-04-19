using ZooApplication.Domain.Events;

namespace ZooApplication.Application.Interfaces;

/// <summary>
/// Domain Event Dispatcher Interface.
/// </summary>
public interface IDomainEventDispatcher
{
    void Dispatch(IEnumerable<IDomainEvent> domainEvents);
}