using MediatR;
using ZooApplication.Domain.Events;

namespace ZooApplication.Application.Handlers;

public class AnimalMovedEventHandler : INotificationHandler<AnimalMovedEvent>
{
    private readonly ILogger<AnimalMovedEventHandler> _logger;

    public AnimalMovedEventHandler(ILogger<AnimalMovedEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(AnimalMovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"AnimalMovedEvent received for animal: {notification.Animal}, " +
                               $"{notification.OldEnclosureId},-> {notification.NewEnclosureId}: " +
                               $"time = {notification.OccurredOn}");
        
        return Task.CompletedTask;
    }
}