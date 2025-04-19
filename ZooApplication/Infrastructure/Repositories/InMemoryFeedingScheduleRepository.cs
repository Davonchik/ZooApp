using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Infrastructure.Repositories;

public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly List<FeedingSchedule> _feedingSchedules = new List<FeedingSchedule>();
    
    public void Add(FeedingSchedule feedingSchedule) => _feedingSchedules.Add(feedingSchedule);
    
    public IEnumerable<FeedingSchedule> GetAll() => _feedingSchedules;
    
    public FeedingSchedule GetById(Guid id) => _feedingSchedules.FirstOrDefault(a => a.Id == id) ?? 
                                               throw new KeyNotFoundException("Feeding schedule not found");
    
    public IEnumerable<FeedingSchedule> GetUpcoming(DateTime from) => _feedingSchedules
        .Where(f => f.FeedingTime.Value >= from);
    
    public void Remove(FeedingSchedule feedingSchedule) => _feedingSchedules.Remove(feedingSchedule);

    public void Update(FeedingSchedule newFeedingScheduleModel, Guid feedingScheduleId)
    {
        var existing = _feedingSchedules.FirstOrDefault(a => a.Id == feedingScheduleId);
        
        if (existing == null)
            throw new KeyNotFoundException("Feeding schedule not found");
        
        newFeedingScheduleModel.Id = existing.Id;
        _feedingSchedules.Remove(existing);
        _feedingSchedules.Add(newFeedingScheduleModel);
    }
}