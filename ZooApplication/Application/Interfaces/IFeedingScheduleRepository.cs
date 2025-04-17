using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

public interface IFeedingScheduleRepository
{
    FeedingSchedule GetById(Guid id);
    
    IEnumerable<FeedingSchedule> GetAll();
    
    IEnumerable<FeedingSchedule> GetUpcoming(DateTime from);
    
    void Add(FeedingSchedule feedingSchedule);
    
    void Remove(FeedingSchedule feedingSchedule);

    public void Update(FeedingSchedule newFeedingScheduleModel, Guid feedingScheduleId);
}