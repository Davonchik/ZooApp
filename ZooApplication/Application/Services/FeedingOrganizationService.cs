using ZooApp.Application.Interfaces;
using ZooApp.Domain.Entities;
using ZooApp.Domain.ValueObjects;

namespace ZooApp.Application.Services;

public class FeedingOrganizationService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public FeedingOrganizationService(IAnimalRepository animalRepository,
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    public FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingDate, string food)
    {
        var animal = _animalRepository.GetById(animalId);

        var feedingTimeVo = new FeedingTime(feedingDate);
        var schedule = new FeedingSchedule(animalId, feedingTimeVo, food);
        _feedingScheduleRepository.Add(schedule);
        return schedule;
    }
}