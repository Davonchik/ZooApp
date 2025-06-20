using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Infrastructure.Repositories;

namespace TestZooApp;

public class InMemoryFeedingScheduleRepositoryTests
{
    private readonly InMemoryFeedingScheduleRepository _repo = new();

    private static FeedingSchedule MakeSchedule(Guid animalId, int minsAhead, string food = "Food") =>
        new FeedingSchedule(
            animalId,
            new FeedingTime(DateTime.UtcNow.AddMinutes(minsAhead)),
            new Food(new Name(food), new AnimalType(AnimalTypeValue.Default))
        );

    [Fact]
    public void AddAndGetAll_ReturnsAddedSchedule()
    {
        // Arrange
        var sched = MakeSchedule(Guid.NewGuid(), 10);

        // Act
        _repo.Add(sched);
        var result = _repo.GetAll();

        // Assert
        Assert.Contains(sched, result);
    }

    [Fact]
    public void GetById_Existing_ReturnsSchedule()
    {
        // Arrange
        var sched = MakeSchedule(Guid.NewGuid(), 15);
        _repo.Add(sched);

        // Act
        var fetched = _repo.GetById(sched.Id);

        // Assert
        Assert.Same(sched, fetched);
    }

    [Fact]
    public void GetById_NonExisting_Throws()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => _repo.GetById(id));
    }

    [Fact]
    public void GetUpcoming_FiltersByDate()
    {
        // Arrange
        var animal = Guid.NewGuid();
        var soon = MakeSchedule(animal, 5);
        var near = MakeSchedule(animal, 20);
        var later = MakeSchedule(animal, 60);
        _repo.Add(soon);
        _repo.Add(near);
        _repo.Add(later);

        var from = DateTime.UtcNow.AddMinutes(30);

        // Act
        var upcoming = _repo.GetUpcoming(from).ToList();

        // Assert
        Assert.DoesNotContain(soon, upcoming);
        Assert.DoesNotContain(near, upcoming);
        Assert.Contains(later, upcoming);
        Assert.Single(upcoming);
    }

    [Fact]
    public void Remove_DeletesSchedule()
    {
        // Arrange
        var sched = MakeSchedule(Guid.NewGuid(), 30);
        _repo.Add(sched);

        // Act
        _repo.Remove(sched);

        // Assert
        Assert.DoesNotContain(sched, _repo.GetAll());
    }

    [Fact]
    public void Update_ReplacesScheduleAndKeepsId()
    {
        // Arrange
        var animal = Guid.NewGuid();
        var original = MakeSchedule(animal, 20, "A");
        _repo.Add(original);

        var updated = MakeSchedule(animal, 40, "B");

        // Act
        _repo.Update(updated, original.Id);
        var fetched = _repo.GetById(original.Id);

        // Assert
        Assert.Equal(original.Id, fetched.Id);
        Assert.Equal("B", fetched.Food.Name.Value);
        Assert.Equal(updated.FeedingTime.Value, fetched.FeedingTime.Value);
    }

    [Fact]
    public void Update_NonExisting_Throws()
    {
        // Arrange
        var sched = MakeSchedule(Guid.NewGuid(), 25);

        // Act & Assert
        var id = Guid.NewGuid();
        var ex = Assert.Throws<KeyNotFoundException>(() => _repo.Update(sched, id));
        Assert.Equal("Feeding schedule not found", ex.Message);
    }
}