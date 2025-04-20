using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class FeedingTimeEventTests
{
    [Fact]
    public void Constructor_SetsFeedingScheduleAndOccurredOn()
    {
        // Arrange
        var animalId = Guid.NewGuid();
        var feedingTime = DateTime.UtcNow.AddMinutes(5);
        var schedule = new FeedingSchedule(
            animalId,
            new FeedingTime(feedingTime),
            new Food(new Name("Bananas"), new AnimalType(AnimalTypeValue.Default))
        );
            
        // Act
        var before = DateTime.UtcNow;
        var evt = new FeedingTimeEvent(schedule);
        var after = DateTime.UtcNow;

        // Assert
        Assert.Same(schedule, evt.FeedingSchedule);
        Assert.InRange(evt.OccurredOn, before, after);
    }
}