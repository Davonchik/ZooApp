using ZooApplication.Domain.Events;

namespace ZooApplication.Domain.Entities;

/// <summary>
/// Event entity.
/// </summary>
public abstract class EventEntity
{
    private List<IDomainEvent> _domainEvents = [];
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Add Domain Event Method.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clear Domain Events Method.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}