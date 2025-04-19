using MediatR;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Events;

namespace ZooApplication.Application.Dispatcher;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Dispatch(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var dEvent in domainEvents)
        {
            _mediator.Publish(dEvent).GetAwaiter().GetResult();
        }
    }
}