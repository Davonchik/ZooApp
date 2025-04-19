using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Domain.Entities;

public class FeedingSchedule
{
    public Guid Id { get; set; }
    public Guid AnimalId { get; private set; }
    public FeedingTime FeedingTime { get; private set; }
    public Food Food { get; private set; }
    public bool IsCompleted { get; private set; }

    public FeedingSchedule(Guid animalId, FeedingTime feedingTime, Food food)
    {
        Id = Guid.NewGuid();
        AnimalId = animalId;
        FeedingTime = feedingTime;
        Food = food;
        IsCompleted = false;
    }

    /// <summary>
    /// Method for rescheduling feeding time.
    /// </summary>
    /// <param name="newFeedingTime">New Feeding Time.</param>
    public void Reschedule(DateTime newFeedingTime)
    {
        FeedingTime = new FeedingTime(newFeedingTime);
    }

    /// <summary>
    /// Method for marking as completed.
    /// </summary>
    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}