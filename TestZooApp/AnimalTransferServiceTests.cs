using Moq;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class AnimalTransferServiceTests
{
    private readonly Mock<IAnimalRepository> _animalRepo;
    private readonly Mock<IEnclosureRepository> _enclosureRepo;
    private readonly Mock<IDomainEventDispatcher> _dispatcher;
    private readonly AnimalTransferService _svc;

    public AnimalTransferServiceTests()
    {
        _animalRepo = new Mock<IAnimalRepository>();
        _enclosureRepo = new Mock<IEnclosureRepository>();
        _dispatcher = new Mock<IDomainEventDispatcher>();

        _svc = new AnimalTransferService(
            _animalRepo.Object,
            _enclosureRepo.Object,
            _dispatcher.Object
        );
    }

    [Fact]
    public void TransferAnimal_WithNoOldEnclosure_AddsToTargetOnly_AndDispatchesNothing()
    {
        // Arrange
        var animal = new Animal(
            new Name("Bob"),
            new AnimalType(AnimalTypeValue.Predator),
            DateTime.UtcNow,
            new Gender(GenderValue.Male),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var target = new Enclosure(
            new Name("Carnivore House"),
            new Capacity(10),
            new AnimalType(AnimalTypeValue.Predator)
        );
        _animalRepo.Setup(r => r.GetById(animal.Id)).Returns(animal);
        _enclosureRepo.Setup(r => r.GetById(target.Id)).Returns(target);
        _enclosureRepo.Setup(r => r.GetAll()).Returns(Array.Empty<Enclosure>());

        List<IDomainEvent> capturedEvents = null;
        _dispatcher
            .Setup(d => d.Dispatch(It.IsAny<IEnumerable<IDomainEvent>>()))
            .Callback<IEnumerable<IDomainEvent>>(evts => capturedEvents = evts.ToList());

        // Act
        _svc.TransferAnimal(animal.Id, target.Id);

        // Assert
        Assert.Contains(animal.Id, target.AnimalIds);
        Assert.NotNull(capturedEvents);
        Assert.Empty(capturedEvents);
        _dispatcher.Verify(d => d.Dispatch(It.IsAny<IEnumerable<IDomainEvent>>()), Times.Once);
    }

    [Fact]
    public void TransferAnimal_TypeMismatch_ThrowsArgumentException()
    {
        // Arrange
        var animal = new Animal(
            new Name("Gerry"),
            new AnimalType(AnimalTypeValue.Herbivores),
            DateTime.UtcNow,
            new Gender(GenderValue.Female),
            new Food(new Name("Grass"), new AnimalType(AnimalTypeValue.Herbivores)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var target = new Enclosure(
            new Name("Bird House"),
            new Capacity(5),
            new AnimalType(AnimalTypeValue.Birds)
        );
        _animalRepo.Setup(r => r.GetById(animal.Id)).Returns(animal);
        _enclosureRepo.Setup(r => r.GetById(target.Id)).Returns(target);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() =>
            _svc.TransferAnimal(animal.Id, target.Id)
        );
        Assert.Equal("Type mismatch!", ex.Message);
    }

    [Fact]
    public void TransferAnimal_WithOldEnclosureAndCapacity_AddsAndRemoves_AndDispatchesMoveEvent()
    {
        // Arrange
        var animal = new Animal(
            new Name("Leo"),
            new AnimalType(AnimalTypeValue.Predator),
            DateTime.UtcNow,
            new Gender(GenderValue.Male),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var oldEnclosure = new Enclosure(
            new Name("OldDen"),
            new Capacity(2),
            new AnimalType(AnimalTypeValue.Predator)
        );
        oldEnclosure.AddAnimal(animal);

        var targetEnclosure = new Enclosure(
            new Name("NewDen"),
            new Capacity(3),
            new AnimalType(AnimalTypeValue.Predator)
        );
        _animalRepo.Setup(r => r.GetById(animal.Id)).Returns(animal);
        _enclosureRepo.Setup(r => r.GetById(targetEnclosure.Id)).Returns(targetEnclosure);
        _enclosureRepo.Setup(r => r.GetAll()).Returns(new[] { oldEnclosure });

        List<IDomainEvent> capturedEvents = null;
        _dispatcher
            .Setup(d => d.Dispatch(It.IsAny<IEnumerable<IDomainEvent>>()))
            .Callback<IEnumerable<IDomainEvent>>(evts => capturedEvents = evts.ToList());

        // Act
        _svc.TransferAnimal(animal.Id, targetEnclosure.Id);

        // Assert
        Assert.Empty(oldEnclosure.AnimalIds);
        Assert.Contains(animal.Id, targetEnclosure.AnimalIds);

        Assert.NotNull(capturedEvents);
        var movedEvent = Assert.Single(capturedEvents.OfType<AnimalMovedEvent>());
        Assert.Equal(animal, movedEvent.Animal);
        Assert.Equal(oldEnclosure.Id, movedEvent.OldEnclosureId);
        Assert.Equal(targetEnclosure.Id, movedEvent.NewEnclosureId);

        _dispatcher.Verify(d => d.Dispatch(It.IsAny<IEnumerable<IDomainEvent>>()), Times.Once);
        Assert.Empty(animal.DomainEvents);
    }
}