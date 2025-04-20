using Moq;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class AnimalServiceTests
{
    private readonly Mock<IAnimalRepository>        _aniRepo;
    private readonly Mock<IEnclosureRepository>     _encRepo;
    private readonly Mock<IAnimalTransferService>   _transferSvc;
    private readonly Mock<IFeedingScheduleRepository> _schedRepo;
    private readonly AnimalService                  _svc;

    public AnimalServiceTests()
    {
        _aniRepo     = new Mock<IAnimalRepository>();
        _encRepo     = new Mock<IEnclosureRepository>();
        _transferSvc = new Mock<IAnimalTransferService>();
        _schedRepo   = new Mock<IFeedingScheduleRepository>();

        _svc = new AnimalService(
            _aniRepo.Object,
            _encRepo.Object,
            _transferSvc.Object,
            _schedRepo.Object
        );
    }

    [Fact]
    public void GetAll_ReturnsAllFromRepository()
    {
        // Arrange
        var expected = new[]
        {
            new Animal(
                new Name("Leo"),
                new AnimalType(AnimalTypeValue.Predator),
                DateTime.UtcNow,
                new Gender(GenderValue.Male),
                new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
                new HealthStatus(HealthStatusValue.Healthy)
            )
        };
        _aniRepo.Setup(r => r.GetAll()).Returns(expected);

        // Act
        var actual = _svc.GetAll();

        // Assert
        Assert.Same(expected, actual);
        _aniRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetById_ReturnsFromRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expected = new Animal(
            new Name("Ella"),
            new AnimalType(AnimalTypeValue.Herbivores),
            DateTime.UtcNow,
            new Gender(GenderValue.Female),
            new Food(new Name("Grass"), new AnimalType(AnimalTypeValue.Herbivores)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        _aniRepo.Setup(r => r.GetById(id)).Returns(expected);

        // Act
        var actual = _svc.GetById(id);

        // Assert
        Assert.Equal(expected, actual);
        _aniRepo.Verify(r => r.GetById(id), Times.Once);
    }

    [Fact]
    public void CreateAnimal_Success_AddsAndTransfers()
    {
        // Arrange
        var animal = new Animal(
            new Name("Max"),
            new AnimalType(AnimalTypeValue.Birds),
            DateTime.UtcNow,
            new Gender(GenderValue.Male),
            new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Birds)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var enclosureId = Guid.NewGuid();

        // Act
        var result = _svc.CreateAnimal(animal, enclosureId);

        // Assert
        Assert.True(result);
        _aniRepo.Verify(r => r.Add(animal), Times.Once);
        _transferSvc.Verify(t => t.TransferAnimal(animal.Id, enclosureId), Times.Once);
    }

    [Fact]
    public void CreateAnimal_TransferThrows_RemovesAnimalAndRethrows()
    {
        // Arrange
        var animal = new Animal(
            new Name("Polly"),
            new AnimalType(AnimalTypeValue.Birds),
            DateTime.UtcNow,
            new Gender(GenderValue.Female),
            new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Birds)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var enclosureId = Guid.NewGuid();
        _transferSvc
            .Setup(t => t.TransferAnimal(animal.Id, enclosureId))
            .Throws(new InvalidOperationException("transfer failed"));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _svc.CreateAnimal(animal, enclosureId));
        _aniRepo.Verify(r => r.Add(animal), Times.Once);
        _aniRepo.Verify(r => r.Remove(animal), Times.Once);
    }

    [Fact]
    public void UpdateAnimal_CallsRepository()
    {
        // Arrange
        var animal = new Animal(
            new Name("Bella"),
            new AnimalType(AnimalTypeValue.Fishes),
            DateTime.UtcNow,
            new Gender(GenderValue.Female),
            new Food(new Name("Algae"), new AnimalType(AnimalTypeValue.Fishes)),
            new HealthStatus(HealthStatusValue.Sick)
        );
        var id = Guid.NewGuid();

        // Act
        var result = _svc.UpdateAnimal(animal, id);

        // Assert
        Assert.True(result);
        _aniRepo.Verify(r => r.Update(animal, id), Times.Once);
    }

    [Fact]
    public void Heal_ThrowsIfHealthy()
    {
        // Arrange
        var id = Guid.NewGuid();
        var healthy = new Animal(
            new Name("Sam"),
            new AnimalType(AnimalTypeValue.Default),
            DateTime.UtcNow,
            new Gender(GenderValue.Male),
            new Food(new Name("Food"), new AnimalType(AnimalTypeValue.Default)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        _aniRepo.Setup(r => r.GetById(id)).Returns(healthy);

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _svc.Heal(id));
        Assert.Contains("already healed", ex.Message);
    }

    [Fact]
    public void Heal_WhenSick_SetsHealthy()
    {
        // Arrange
        var id = Guid.NewGuid();
        var sick = new Animal(
            new Name("Sam"),
            new AnimalType(AnimalTypeValue.Default),
            DateTime.UtcNow,
            new Gender(GenderValue.Female),
            new Food(new Name("Food"), new AnimalType(AnimalTypeValue.Default)),
            new HealthStatus(HealthStatusValue.Sick)
        );
        _aniRepo.Setup(r => r.GetById(id)).Returns(sick);

        // Act
        _svc.Heal(id);

        // Assert
        Assert.Equal(HealthStatusValue.Healthy, sick.HealthStatus.Value);
    }

    [Fact]
    public void Feed_CallsFeedWithoutException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var animal = new Animal(
            new Name("Nemo"),
            new AnimalType(AnimalTypeValue.Fishes),
            DateTime.UtcNow,
            new Gender(GenderValue.Male),
            new Food(new Name("Plankton"), new AnimalType(AnimalTypeValue.Fishes)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        _aniRepo.Setup(r => r.GetById(id)).Returns(animal);

        // Act
        _svc.Feed(id);

        // Assert
        // (no exception = pass)
    }

    [Fact]
    public void DeleteAnimal_RemovesEnclosureSchedulesAndAnimal()
    {
        // Arrange
        var animal = new Animal(
            new Name("Zara"),
            new AnimalType(AnimalTypeValue.Birds),
            DateTime.UtcNow,
            new Gender(GenderValue.Female),
            new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Birds)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var id = animal.Id;
        _aniRepo.Setup(r => r.GetById(id)).Returns(animal);

        var enclosure = new Enclosure(
            new Name("Aviary"),
            new Capacity(5),
            new AnimalType(AnimalTypeValue.Birds)
        );
        enclosure.AddAnimal(animal);
        _encRepo.Setup(r => r.GetAll()).Returns(new[] { enclosure });

        var future1 = DateTime.UtcNow.AddMinutes(10);
        var future2 = DateTime.UtcNow.AddHours(1);
        var s1 = new FeedingSchedule(
            id,
            new FeedingTime(future1),
            new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Birds))
        );
        var s2 = new FeedingSchedule(
            id,
            new FeedingTime(future2),
            new Food(new Name("Worms"), new AnimalType(AnimalTypeValue.Birds))
        );
        _schedRepo.Setup(r => r.GetAll()).Returns(new[] { s1, s2 });

        // Act
        _svc.DeleteAnimal(id);

        // Assert
        Assert.Empty(enclosure.AnimalIds);
        _schedRepo.Verify(r => r.Remove(s1), Times.Once);
        _schedRepo.Verify(r => r.Remove(s2), Times.Once);
        _aniRepo.Verify(r => r.Remove(animal), Times.Once);
    }
}