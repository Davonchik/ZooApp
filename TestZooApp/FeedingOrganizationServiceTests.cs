using Moq;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class FeedingOrganizationServiceTests
{
    private readonly Mock<IAnimalRepository> _animalRepo;
    private readonly Mock<IFeedingScheduleRepository> _schedRepo;
    private readonly FeedingOrganizationService _svc;

    public FeedingOrganizationServiceTests()
    {
        _animalRepo = new Mock<IAnimalRepository>();
        _schedRepo = new Mock<IFeedingScheduleRepository>();
        _svc = new FeedingOrganizationService(
            _animalRepo.Object,
            _schedRepo.Object
        );
    }

    [Fact]
    public void GetAll_ReturnsAllSchedulesFromRepository()
    {
        // Arrange
        var list = new[]
        {
            new FeedingSchedule(
                Guid.NewGuid(),
                new FeedingTime(DateTime.UtcNow.AddHours(1)),
                new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Birds))
            )
        };
        _schedRepo.Setup(r => r.GetAll()).Returns(list);

        // Act
        var result = _svc.GetAll();

        // Assert
        Assert.Same(list, result);
        _schedRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetById_ReturnsScheduleFromRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var schedule = new FeedingSchedule(
            id,
            new FeedingTime(DateTime.UtcNow.AddHours(2)),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator))
        );
        _schedRepo.Setup(r => r.GetById(id)).Returns(schedule);

        // Act
        var result = _svc.GetById(id);

        // Assert
        Assert.Equal(schedule, result);
        _schedRepo.Verify(r => r.GetById(id), Times.Once);
    }

    [Fact]
    public void CreateFeedingSchedule_AddsAndReturns_WhenNoDuplicate()
    {
        // Arrange
        var schedule = new FeedingSchedule(
            Guid.NewGuid(),
            new FeedingTime(DateTime.UtcNow.AddHours(3)),
            new Food(new Name("Grass"), new AnimalType(AnimalTypeValue.Herbivores))
        );
        _schedRepo.Setup(r => r.GetAll()).Returns(Array.Empty<FeedingSchedule>());

        // Act
        var result = _svc.CreateFeedingSchedule(schedule);

        // Assert
        Assert.Equal(schedule, result);
        _schedRepo.Verify(r => r.Add(schedule), Times.Once);
    }

    [Fact]
    public void CreateFeedingSchedule_Throws_WhenDuplicateExists()
    {
        // Arrange
        var time = DateTime.UtcNow.AddHours(4);
        var animalId = Guid.NewGuid();
        var existing = new FeedingSchedule(
            animalId,
            new FeedingTime(time),
            new Food(new Name("Fish"), new AnimalType(AnimalTypeValue.Fishes))
        );
        _schedRepo.Setup(r => r.GetAll()).Returns(new[] { existing });

        var duplicate = new FeedingSchedule(
            animalId,
            new FeedingTime(time),
            new Food(new Name("Fish"), new AnimalType(AnimalTypeValue.Fishes))
        );

        // Act & Assert
        var ex = Assert.Throws<ApplicationException>(() =>
            _svc.CreateFeedingSchedule(duplicate)
        );
        Assert.Equal("Feeding schedule already exists", ex.Message);
        _schedRepo.Verify(r => r.Add(It.IsAny<FeedingSchedule>()), Times.Never);
    }

    [Fact]
    public void Reschedule_UpdatesTimeAndCallsUpdate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var originalTime = DateTime.UtcNow.AddHours(1);
        var newTime = originalTime.AddHours(2);
        var schedule = new FeedingSchedule(
            id,
            new FeedingTime(originalTime),
            new Food(new Name("Corn"), new AnimalType(AnimalTypeValue.Herbivores))
        );
        _schedRepo.Setup(r => r.GetById(id)).Returns(schedule);

        // Act
        var result = _svc.Reschedule(id, newTime);

        // Assert
        Assert.Equal(newTime, result.FeedingTime.Value);
        _schedRepo.Verify(r => r.Update(schedule, id), Times.Once);
    }

    [Fact]
    public void UpdateSchedule_CallsGetAnimalAndUpdateRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var schedule = new FeedingSchedule(
            id,
            new FeedingTime(DateTime.UtcNow.AddHours(5)),
            new Food(new Name("Worms"), new AnimalType(AnimalTypeValue.Birds))
        );
        _animalRepo.Setup(r => r.GetById(schedule.AnimalId))
                   .Returns(new Animal(
                       new Name("Benny"),
                       new AnimalType(AnimalTypeValue.Birds),
                       DateTime.UtcNow.AddYears(-1),
                       new Gender(GenderValue.Male),
                       new Food(new Name("Worms"), new AnimalType(AnimalTypeValue.Birds)),
                       new HealthStatus(HealthStatusValue.Healthy)
                   ));

        // Act
        _svc.UpdateSchedule(schedule, id);

        // Assert
        _animalRepo.Verify(r => r.GetById(schedule.AnimalId), Times.Once);
        _schedRepo.Verify(r => r.Update(schedule, id), Times.Once);
    }

    [Fact]
    public void ScheduleFeeding_CreatesAndAddsNewSchedule()
    {
        // Arrange
        var animalId = Guid.NewGuid();
        var animal = new Animal(
            new Name("Sammy"),
            new AnimalType(AnimalTypeValue.Default),
            DateTime.UtcNow.AddYears(-2),
            new Gender(GenderValue.Male),
            new Food(new Name("Pellets"), new AnimalType(AnimalTypeValue.Default)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        _animalRepo.Setup(r => r.GetById(animalId)).Returns(animal);
        _schedRepo.Setup(r => r.GetAll()).Returns(Array.Empty<FeedingSchedule>());

        // Act
        var result = _svc.ScheduleFeeding(animalId, DateTime.UtcNow.AddHours(6), "Pellets");

        // Assert
        Assert.Equal(animalId, result.AnimalId);
        Assert.Equal("Pellets", result.Food.Name.Value);
        _schedRepo.Verify(r => r.Add(result), Times.Once);
    }

    [Fact]
    public void DeleteSchedule_CallsRemoveOnRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var schedule = new FeedingSchedule(
            id,
            new FeedingTime(DateTime.UtcNow.AddHours(7)),
            new Food(new Name("Fish"), new AnimalType(AnimalTypeValue.Fishes))
        );
        _schedRepo.Setup(r => r.GetById(id)).Returns(schedule);

        // Act
        _svc.DeleteSchedule(id);

        // Assert
        _schedRepo.Verify(r => r.Remove(schedule), Times.Once);
    }

    [Fact]
    public void CompleteSchedule_MarksCompletedAndUpdates()
    {
        // Arrange
        var id = Guid.NewGuid();
        var schedule = new FeedingSchedule(
            id,
            new FeedingTime(DateTime.UtcNow.AddHours(8)),
            new Food(new Name("Leaves"), new AnimalType(AnimalTypeValue.Herbivores))
        );
        _schedRepo.Setup(r => r.GetById(id)).Returns(schedule);

        // Act
        var result = _svc.CompleteSchedule(id);

        // Assert
        Assert.True(result.IsCompleted);
        _schedRepo.Verify(r => r.Update(schedule, id), Times.Once);
    }
}