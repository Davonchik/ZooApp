using ZooApplication.Domain.Entities;

namespace ZooApplication.Domain.Events;

public class FeedingTimeEvent : IDomainEvent
{
    public FeedingSchedule FeedingSchedule { get; private set; }
    public DateTime OccurredOn { get; private set; }

    public FeedingTimeEvent(FeedingSchedule feedingSchedule)
    {
        FeedingSchedule = feedingSchedule;
        OccurredOn = DateTime.UtcNow;
    }
}