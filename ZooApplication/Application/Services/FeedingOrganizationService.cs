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
    
    public IEnumerable<FeedingSchedule> GetAll() => _feedingScheduleRepository.GetAll();
    
    public FeedingSchedule GetById(Guid id) => _feedingScheduleRepository.GetById(id);

    public FeedingSchedule CreateFeedingSchedule(FeedingSchedule feedingSchedule)
    {
        _feedingScheduleRepository.Add(feedingSchedule);
        return feedingSchedule;
    }
    
    public FeedingSchedule Reschedule(Guid id, DateTime newFeedingTime)
    {
        var schedule = _feedingScheduleRepository.GetById(id);
        schedule.Reschedule(newFeedingTime);
        _feedingScheduleRepository.Update(schedule, id);
        return schedule;
    }
    
    public void UpdateSchedule(FeedingSchedule newScheduleModel, Guid scheduleId)
    {
        _feedingScheduleRepository.Update(newScheduleModel, scheduleId);
    }

    public FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingDate, Food food)
    {
        var animal = _animalRepository.GetById(animalId);

        var feedingTimeVo = new FeedingTime(feedingDate);
        var schedule = new FeedingSchedule(animalId, feedingTimeVo, food);
        _feedingScheduleRepository.Add(schedule);
        return schedule;
    }
    
    public void DeleteSchedule(Guid id)
    {
        var existing = _feedingScheduleRepository.GetById(id);
        _feedingScheduleRepository.Remove(existing);
    }
    
    public FeedingSchedule CompleteSchedule(Guid id)
    {
        var existing = _feedingScheduleRepository.GetById(id);
        existing.MarkAsCompleted();
        _feedingScheduleRepository.Update(existing, id);
        return existing;
    }
}