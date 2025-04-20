using MediatR;
using Moq;
using ZooApplication.Application.Dispatcher;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class DomainEventDispatcherTests
{
    private static IDomainEvent MakeEvent() =>
        new AnimalMovedEvent(
            new Animal(
                new Name("A"),
                new AnimalType(AnimalTypeValue.Default),
                System.DateTime.UtcNow,
                new Gender(GenderValue.Male),
                new Food(new Name("F"), new AnimalType(AnimalTypeValue.Default)),
                new HealthStatus(HealthStatusValue.Healthy)
            ),
            System.Guid.NewGuid(),
            System.Guid.NewGuid()
        );

    [Fact]
    public void Dispatch_CallsMediatorPublish_ForEachEvent()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        var dispatcher = new DomainEventDispatcher(mediator.Object);
        var ev1 = MakeEvent();
        var ev2 = MakeEvent();
        var events = new List<IDomainEvent> { ev1, ev2 };

        // Act
        dispatcher.Dispatch(events);

        // Assert
        mediator.Verify(m => m.Publish(ev1, It.IsAny<CancellationToken>()), Times.Once);
        mediator.Verify(m => m.Publish(ev2, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Dispatch_WithEmptyCollection_DoesNotCallPublish()
    {
        // Arrange
        var mediator = new Mock<IMediator>();
        var dispatcher = new DomainEventDispatcher(mediator.Object);
        var events = new List<IDomainEvent>();

        // Act
        dispatcher.Dispatch(events);

        // Assert
        mediator.Verify(m => m.Publish(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}