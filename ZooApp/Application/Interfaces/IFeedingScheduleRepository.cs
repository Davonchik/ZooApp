using ZooApp.Domain.Entities;

namespace ZooApp.Application.Interfaces;

public interface IFeedingScheduleRepository
{
    FeedingSchedule GetById(Guid id);
    IEnumerable<FeedingSchedule> GetAll();
    IEnumerable<FeedingSchedule> GetUpcoming(DateTime from);
    void Add(FeedingSchedule feedingSchedule);
    void Remove(FeedingSchedule feedingSchedule);
}