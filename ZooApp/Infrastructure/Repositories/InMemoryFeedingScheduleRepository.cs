using ZooApp.Application.Interfaces;
using ZooApp.Domain.Entities;

namespace ZooApp.Infrastructure.Repositories;

public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly List<FeedingSchedule> _feedingSchedules = new List<FeedingSchedule>();
    public void Add(FeedingSchedule feedingSchedule) => _feedingSchedules.Add(feedingSchedule);
    public IEnumerable<FeedingSchedule> GetAll() => _feedingSchedules;
    public FeedingSchedule GetById(Guid id) => _feedingSchedules.FirstOrDefault(a => a.Id == id);
    public IEnumerable<FeedingSchedule> GetUpcoming(DateTime from) => _feedingSchedules
        .Where(f => f.FeedingTime.Value >= from);
    public void Remove(FeedingSchedule feedingSchedule) => _feedingSchedules.Remove(feedingSchedule);
}