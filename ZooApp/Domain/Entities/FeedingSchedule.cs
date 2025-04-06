using ZooApp.Domain.ValueObjects;

namespace ZooApp.Domain.Entities;

public class FeedingSchedule
{
    public Guid Id { get; private set; }
    public Guid AnimalId { get; private set; }
    public FeedingTime FeedingTime { get; private set; }
    public string Food { get; private set; }
    public bool IsCompleted { get; private set; }

    public FeedingSchedule(Guid animalId, FeedingTime feedingTime, string food)
    {
        Id = Guid.NewGuid();
        AnimalId = animalId;
        FeedingTime = feedingTime;
        Food = food;
        IsCompleted = false;
    }

    public void Complete()
    {
        IsCompleted = true;
    }
}