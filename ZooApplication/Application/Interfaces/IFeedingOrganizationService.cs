using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Application.Interfaces;

/// <summary>
/// Interface of Feeding Organization Service.
/// </summary>
public interface IFeedingOrganizationService
{
    IEnumerable<FeedingSchedule> GetAll();

    FeedingSchedule GetById(Guid id);

    FeedingSchedule CreateFeedingSchedule(FeedingSchedule feedingSchedule);

    FeedingSchedule Reschedule(Guid id, DateTime newFeedingTime);
    
    FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingDate, string food);

    void UpdateSchedule(FeedingSchedule newScheduleModel, Guid scheduleId);

    void DeleteSchedule(Guid id);

    FeedingSchedule CompleteSchedule(Guid id);
}