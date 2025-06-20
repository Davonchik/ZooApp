using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Application.Services;

/// <summary>
/// Feeding Organization Service.
/// </summary>
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

    /// <summary>
    /// Method for getting all Feeding Schedules.
    /// </summary>
    /// <returns>IEnumerable of Feeding Schedules.</returns>
    public IEnumerable<FeedingSchedule> GetAll() => _feedingScheduleRepository.GetAll();

    /// <summary>
    /// Method for getting Feeding Schedule by ID.
    /// </summary>
    /// <param name="id">Feeding Schedule's ID.</param>
    /// <returns>Feeding Schedule.</returns>
    public FeedingSchedule GetById(Guid id) => _feedingScheduleRepository.GetById(id);

    /// <summary>
    /// Method for Feeding Schedule creation.
    /// </summary>
    /// <param name="feedingSchedule">Feeding Schedule.</param>
    /// <returns>Feeding Schedule.</returns>
    public FeedingSchedule CreateFeedingSchedule(FeedingSchedule feedingSchedule)
    {
        if (_feedingScheduleRepository.GetAll().Any(a => a.FeedingTime.Value ==
                                                         feedingSchedule.FeedingTime.Value
            && a.AnimalId == feedingSchedule.AnimalId))
            throw new ApplicationException("Feeding schedule already exists");
        _feedingScheduleRepository.Add(feedingSchedule);
        return feedingSchedule;
    }

    /// <summary>
    /// Method for rescheduling.
    /// </summary>
    /// <param name="id">Feeding Schedule's ID.</param>
    /// <param name="newFeedingTime">New Feeding Schedule.</param>
    /// <returns>Feeding Schedule.</returns>
    public FeedingSchedule Reschedule(Guid id, DateTime newFeedingTime)
    {
        var schedule = _feedingScheduleRepository.GetById(id);
        schedule.Reschedule(newFeedingTime);
        _feedingScheduleRepository.Update(schedule, id);
        return schedule;
    }

    /// <summary>
    /// Method for Schedule Info updating.
    /// </summary>
    /// <param name="newScheduleModel">New Feeding Schedule.</param>
    /// <param name="scheduleId">Feeding Schedule's ID.</param>
    public void UpdateSchedule(FeedingSchedule newScheduleModel, Guid scheduleId)
    {
        _animalRepository.GetById(newScheduleModel.AnimalId);

        _feedingScheduleRepository.Update(newScheduleModel, scheduleId);
    }

    /// <summary>
    /// Schedule Feeding method - creation.
    /// </summary>
    /// <param name="animalId">Feeding Schedule's ID</param>
    /// <param name="feedingDate">Feeding Date.</param>
    /// <param name="food">Food.</param>
    /// <returns>Feeding Schedule.</returns>
    public FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingDate, string food)
    {
        var animal = _animalRepository.GetById(animalId);

        var feedingTimeVo = new FeedingTime(feedingDate);
        var foodVo = new Food(
            new Name(food),
            new AnimalType(animal.Species.Value)
        );

        var schedule = new FeedingSchedule(animalId, feedingTimeVo, foodVo);

        return CreateFeedingSchedule(schedule);
    }

    /// <summary>
    /// Method for Schedule deletion.
    /// </summary>
    /// <param name="id">Feeding Schedule's ID.</param>
    public void DeleteSchedule(Guid id)
    {
        var existing = _feedingScheduleRepository.GetById(id);
        _feedingScheduleRepository.Remove(existing);
    }

    /// <summary>
    /// Method for making feeding process in Time.
    /// </summary>
    /// <param name="id">Feeding Schedule's ID.</param>
    /// <returns>Feeding Schedule.</returns>
    public FeedingSchedule CompleteSchedule(Guid id)
    {
        var existing = _feedingScheduleRepository.GetById(id);
        existing.MarkAsCompleted();
        _feedingScheduleRepository.Update(existing, id);
        return existing;
    }
}