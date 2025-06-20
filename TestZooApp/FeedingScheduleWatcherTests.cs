using System.Reflection;
using Microsoft.Extensions.Hosting;
using Moq;
using ZooApplication.Application.BackgroundServices;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

file sealed class FeedingScheduleWatcherProbe : FeedingScheduleWatcher
{
    public FeedingScheduleWatcherProbe(
        IFeedingScheduleRepository repo,
        IDomainEventDispatcher disp) : base(repo, disp) { }

    public void RunSingleIteration() =>
        typeof(FeedingScheduleWatcher)
            .GetMethod("ExecuteAsync",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
            .Invoke(this, new object?[] { new CancellationToken(true) });
}

public class FeedingScheduleWatcherTests
{
    private static FeedingSchedule MakeDueSchedule(Guid animalId)
    {
        var ft = new FeedingTime(DateTime.UtcNow.AddMilliseconds(10));
        return new FeedingSchedule(
            animalId,
            ft,
            new Food(new Name("Seed"), new AnimalType(AnimalTypeValue.Default)));
    }

    [Fact]
    public async Task RunSingleIteration_DispatchesEventsOnlyForDueAndUncompleted()
    {
        // Arrange
        var id = Guid.NewGuid();
        var due = new FeedingSchedule(
            id,
            new FeedingTime(DateTime.UtcNow.AddSeconds(1)),
            new Food(new Name("Seed"), new AnimalType(AnimalTypeValue.Default)));

        var future = new FeedingSchedule(
            id,
            new FeedingTime(DateTime.UtcNow.AddMinutes(1)),
            new Food(new Name("Later"), new AnimalType(AnimalTypeValue.Default)));

        var done = new FeedingSchedule(
            id,
            new FeedingTime(DateTime.UtcNow.AddSeconds(1)),
            new Food(new Name("Seed"), new AnimalType(AnimalTypeValue.Default)));
        done.MarkAsCompleted();

        var repoMock = new Mock<IFeedingScheduleRepository>();
        repoMock.Setup(r => r.GetAll()).Returns(new[] { due, future, done });

        var dispMock = new Mock<IDomainEventDispatcher>();

        var watcher = new FeedingScheduleWatcher(repoMock.Object, dispMock.Object);

        // Act
        await watcher.StartAsync(default);

        await Task.Delay(TimeSpan.FromSeconds(32));

        await watcher.StopAsync(default);

        // Assert
        dispMock.Verify(d => d.Dispatch(
                It.Is<IEnumerable<IDomainEvent>>(l =>
                    l.Count() == 1 &&
                    l.First().GetType() == typeof(FeedingTimeEvent) &&
                    ((FeedingTimeEvent)l.First()).FeedingSchedule == due)),
            Times.Once);
    }
}