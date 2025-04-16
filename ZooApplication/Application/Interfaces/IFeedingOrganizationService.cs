using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

public interface IFeedingOrganizationService
{
    FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingDate, string food);
}