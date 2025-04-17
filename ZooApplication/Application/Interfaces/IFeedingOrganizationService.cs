using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Application.Interfaces;

public interface IFeedingOrganizationService
{
    FeedingSchedule ScheduleFeeding(Guid animalId, DateTime feedingDate, Food food);
}