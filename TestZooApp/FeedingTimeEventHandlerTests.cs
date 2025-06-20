using Microsoft.Extensions.Logging;
using Moq;
using ZooApplication.Application.Handlers;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class FeedingTimeEventHandlerTests
{
    private static Animal MakeAnimal(Guid id) =>
        new(
            new Name("Leo"),
            new AnimalType(AnimalTypeValue.Predator),
            DateTime.UtcNow.AddYears(-2),
            new Gender(GenderValue.Male),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
            new HealthStatus(HealthStatusValue.Healthy))
        { Id = id };

    private static FeedingSchedule MakeSchedule(Guid animalId) =>
        new(
            animalId,
            new FeedingTime(DateTime.UtcNow.AddMinutes(1)),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)));

    [Fact]
    public async Task Handle_FeedsAnimal_LogsAndMarksScheduleCompleted()
    {
        // Arrange
        var animalId = Guid.NewGuid();
        var animal = MakeAnimal(animalId);
        var schedule = MakeSchedule(animalId);

        var aniRepoMock = new Mock<IAnimalRepository>();
        aniRepoMock.Setup(r => r.GetById(animalId)).Returns(animal);

        var schRepoMock = new Mock<IFeedingScheduleRepository>();
        schRepoMock.Setup(r => r.GetById(schedule.Id)).Returns(schedule);

        var loggerMock = new Mock<ILogger<FeedingTimeEvent>>();

        var handler = new FeedingTimeEventHandler(
            aniRepoMock.Object,
            schRepoMock.Object,
            loggerMock.Object);

        var evt = new FeedingTimeEvent(schedule);
        var token = CancellationToken.None;

        // Act
        await handler.Handle(evt, token);

        // Assert
        aniRepoMock.Verify(r => r.GetById(animalId), Times.Once);

        schRepoMock.Verify(r => r.GetById(schedule.Id), Times.Once);
        schRepoMock.Verify(r => r.Update(schedule, schedule.Id), Times.Once);
        Assert.True(schedule.IsCompleted);

        loggerMock.Verify(l =>
            l.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) =>
                    v.ToString()!.Contains(animal.Name.Value) &&
                    v.ToString()!.Contains(schedule.Food.Name.Value)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}