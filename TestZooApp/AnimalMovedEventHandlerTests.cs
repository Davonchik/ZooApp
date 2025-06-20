using Microsoft.Extensions.Logging;
using Moq;
using ZooApplication.Application.Handlers;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class AnimalMovedEventHandlerTests
{
    private static Animal MakeAnimal() =>
        new(
            new Name("Leo"),
            new AnimalType(AnimalTypeValue.Predator),
            DateTime.UtcNow.AddYears(-2),
            new Gender(GenderValue.Male),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
            new HealthStatus(HealthStatusValue.Healthy));

    [Fact]
    public async Task Handle_WritesInformationLog()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<AnimalMovedEventHandler>>();
        var handler = new AnimalMovedEventHandler(loggerMock.Object);

        var animal = MakeAnimal();
        var oldEnclosureId = Guid.NewGuid();
        var newEnclosureId = Guid.NewGuid();
        var evt = new AnimalMovedEvent(animal, oldEnclosureId, newEnclosureId);

        // Act
        await handler.Handle(evt, CancellationToken.None);

        // Assert – один вызов с уровнем Information
        var invocation = loggerMock.Invocations.Single();
        var logLevel = (LogLevel)invocation.Arguments[0];
        var state = invocation.Arguments[2]?.ToString() ?? string.Empty;

        Assert.Equal(LogLevel.Information, logLevel);
        Assert.Contains(oldEnclosureId.ToString(), state);
        Assert.Contains(newEnclosureId.ToString(), state);
    }
}