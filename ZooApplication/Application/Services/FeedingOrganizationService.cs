using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Application.Services;

public class FeedingOrganizationService : IFeedingOrganizationService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public FeedingOrganizationService(IAnimalRepository animalRepository,
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    public FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingDate, Food food)
    {
        var animal = _animalRepository.GetById(animalId);

        var feedingTimeVo = new FeedingTime(feedingDate);
        var schedule = new FeedingSchedule(animalId, feedingTimeVo, food);
        _feedingScheduleRepository.Add(schedule);
        return schedule;
    }
}