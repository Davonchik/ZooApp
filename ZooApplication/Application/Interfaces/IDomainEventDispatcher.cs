using ZooApplication.Domain.Events;

namespace ZooApplication.Application.Interfaces;

public interface IDomainEventDispatcher
{
    void Dispatch(IEnumerable<IDomainEvent> domainEvents);
}