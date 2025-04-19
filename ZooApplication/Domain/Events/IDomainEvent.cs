using MediatR;

namespace ZooApplication.Domain.Events;

/// <summary>
/// Abstraction for events.
/// </summary>
public interface IDomainEvent : INotification { }