using ZooApp.Domain.Entities;

namespace ZooApp.Domain.Events;

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