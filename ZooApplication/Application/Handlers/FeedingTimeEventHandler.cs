using System.Runtime.InteropServices.ComTypes;
using MediatR;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Events;

namespace ZooApplication.Application.Handlers;

public class FeedingTimeEventHandler : INotificationHandler<FeedingTimeEvent>
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public FeedingTimeEventHandler(IAnimalRepository animalRepository, 
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    public Task Handle(FeedingTimeEvent notification, CancellationToken cancellationToken)
    {
        var animal = _animalRepository.GetById(notification.FeedingSchedule.AnimalId);
        animal.Feed();
        
        var schedule = _feedingScheduleRepository.GetById(notification.FeedingSchedule.Id);
        schedule.MarkAsCompleted();
        
        _feedingScheduleRepository.Update(schedule, notification.FeedingSchedule.Id);
        
        return Task.CompletedTask;
    }
}