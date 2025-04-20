using Moq;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class ZooStatisticsServiceTests
{
    private readonly Mock<IAnimalRepository>           _animalRepo;
    private readonly Mock<IEnclosureRepository>        _enclosureRepo;
    private readonly Mock<IFeedingScheduleRepository> _schedRepo;
    private readonly ZooStatisticsService              _svc;

    public ZooStatisticsServiceTests()
    {
        _animalRepo    = new Mock<IAnimalRepository>();
        _enclosureRepo = new Mock<IEnclosureRepository>();
        _schedRepo     = new Mock<IFeedingScheduleRepository>();

        _svc = new ZooStatisticsService(
            _animalRepo.Object,
            _enclosureRepo.Object,
            _schedRepo.Object
        );
    }

    [Fact]
    public void GetZooStatistics_EmptyRepositories_ReturnsZeros()
    {
        _animalRepo.Setup(r => r.GetAll()).Returns(Array.Empty<Animal>());
        _enclosureRepo.Setup(r => r.GetAll()).Returns(Array.Empty<Enclosure>());
        _schedRepo.Setup(r => r.GetAll()).Returns(Array.Empty<FeedingSchedule>());

        var dto = _svc.GetZooStatistics();

        Assert.Equal(0, dto.TotalAnimals);
        Assert.Equal(0, dto.TotalEnclosures);
        Assert.Equal(0, dto.EmptyEnclosures);
        Assert.Empty(dto.AnimalsByEnclosure);
        Assert.Equal(0, dto.UpcomingFeedings);
    }

    [Fact]
    public void GetZooStatistics_WithData_ComputesCorrectly()
    {
        // Arrange
        var animal1 = new Animal(
            new Name("A1"),
            new AnimalType(AnimalTypeValue.Default),
            DateTime.UtcNow.AddYears(-1),
            new Gender(GenderValue.Male),
            new Food(new Name("Food1"), new AnimalType(AnimalTypeValue.Default)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var animal2 = new Animal(
            new Name("A2"),
            new AnimalType(AnimalTypeValue.Default),
            DateTime.UtcNow.AddYears(-2),
            new Gender(GenderValue.Female),
            new Food(new Name("Food2"), new AnimalType(AnimalTypeValue.Default)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        _animalRepo.Setup(r => r.GetAll()).Returns(new[] { animal1, animal2 });

        var enclosure1 = new Enclosure(
            new Name("First"),
            new Capacity(5),
            new AnimalType(AnimalTypeValue.Default)
        );
        enclosure1.AddAnimal(animal1);

        var enclosure2 = new Enclosure(
            new Name("Second"),
            new Capacity(3),
            new AnimalType(AnimalTypeValue.Default)
        );

        _enclosureRepo.Setup(r => r.GetAll()).Returns(new[] { enclosure1, enclosure2 });

        var now = DateTime.UtcNow;
        var future1 = now.AddHours(1);
        var future2 = now.AddHours(2);

        var s1 = new FeedingSchedule(
            animal1.Id,
            new FeedingTime(future1),
            new Food(new Name("Seed"), new AnimalType(AnimalTypeValue.Default))
        );
        var s3 = new FeedingSchedule(
            animal2.Id,
            new FeedingTime(future2),
            new Food(new Name("Pellet"), new AnimalType(AnimalTypeValue.Default))
        );
        _schedRepo.Setup(r => r.GetAll()).Returns(new[] { s1, s3 });

        // Act
        var dto = _svc.GetZooStatistics();

        // Assert
        Assert.Equal(2, dto.TotalAnimals);
        Assert.Equal(2, dto.TotalEnclosures);
        Assert.Equal(1, dto.EmptyEnclosures);
        Assert.Equal(2, dto.UpcomingFeedings);

        Assert.Equal(2, dto.AnimalsByEnclosure.Count);
        Assert.Equal(1, dto.AnimalsByEnclosure["First"]);
        Assert.Equal(0, dto.AnimalsByEnclosure["Second"]);
    }
}