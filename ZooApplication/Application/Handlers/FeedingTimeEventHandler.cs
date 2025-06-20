using System.Runtime.InteropServices.ComTypes;
using MediatR;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Events;

namespace ZooApplication.Application.Handlers;

/// <summary>
/// Handler for Feeding Time Event.
/// </summary>
public class FeedingTimeEventHandler : INotificationHandler<FeedingTimeEvent>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;
    private readonly ILogger<FeedingTimeEvent> _logger;

    public FeedingTimeEventHandler(IAnimalRepository animalRepository,
        IFeedingScheduleRepository feedingScheduleRepository,
        ILogger<FeedingTimeEvent> logger)
    {
        _animalRepository = animalRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
        _logger = logger;
    }

    public Task Handle(FeedingTimeEvent notification, CancellationToken cancellationToken)
    {
        var animal = _animalRepository.GetById(notification.FeedingSchedule.AnimalId);
        animal.Feed();

        _logger.LogInformation($"Animal {animal.Name} was fed at {notification.OccurredOn} with " +
                               $"{notification.FeedingSchedule.Food}");

        var schedule = _feedingScheduleRepository.GetById(notification.FeedingSchedule.Id);
        schedule.MarkAsCompleted();

        _feedingScheduleRepository.Update(schedule, notification.FeedingSchedule.Id);

        return Task.CompletedTask;
    }
}