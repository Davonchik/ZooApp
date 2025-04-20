using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class EnclosureEntityTests
{
    [Fact]
    public void AddAnimal_Succeeds_WhenTypeMatchesAndCapacityAvailable()
    {
        // Arrange
        var enclosure = new Enclosure(
            new Name("Herbivore House"),
            new Capacity(2),
            new AnimalType(AnimalTypeValue.Herbivores)
        );
        var animal = new Animal(
            new Name("Betsy"),
            new AnimalType(AnimalTypeValue.Herbivores),
            DateTime.UtcNow.AddYears(-3),
            new Gender(GenderValue.Female),
            new Food(new Name("Grass"), new AnimalType(AnimalTypeValue.Herbivores)),
            new HealthStatus(HealthStatusValue.Healthy)
        );

        // Act
        enclosure.AddAnimal(animal);

        // Assert
        Assert.Contains(animal.Id, enclosure.AnimalIds);
        Assert.Equal(1, enclosure.CurrentAnimalCount);
    }

    [Fact]
    public void AddAnimal_ThrowsArgumentException_WhenTypeMismatch()
    {
        // Arrange
        var enclosure = new Enclosure(
            new Name("Bird Aviary"),
            new Capacity(5),
            new AnimalType(AnimalTypeValue.Birds)
        );
        var animal = new Animal(
            new Name("Simba"),
            new AnimalType(AnimalTypeValue.Predator),
            DateTime.UtcNow.AddYears(-2),
            new Gender(GenderValue.Male),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
            new HealthStatus(HealthStatusValue.Healthy)
        );

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => enclosure.AddAnimal(animal));
        Assert.Equal("Type mismatch!", ex.Message);
    }

    [Fact]
    public void AddAnimal_ThrowsInvalidOperationException_WhenCapacityExceeded()
    {
        // Arrange
        var enclosure = new Enclosure(
            new Name("Small Aviary"),
            new Capacity(1),
            new AnimalType(AnimalTypeValue.Birds)
        );
        var first = new Animal(
            new Name("Tweetie"),
            new AnimalType(AnimalTypeValue.Birds),
            DateTime.UtcNow.AddYears(-1),
            new Gender(GenderValue.Female),
            new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Birds)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var second = new Animal(
            new Name("Chirpy"),
            new AnimalType(AnimalTypeValue.Birds),
            DateTime.UtcNow.AddYears(-1),
            new Gender(GenderValue.Male),
            new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Birds)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        enclosure.AddAnimal(first);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => enclosure.AddAnimal(second));
        Assert.Equal("Cannot add more animals to enclosure!", ex.Message);
    }

    [Fact]
    public void AddAnimal_ThrowsInvalidOperationException_WhenDuplicate()
    {
        // Arrange
        var enclosure = new Enclosure(
            new Name("Reptile House"),
            new Capacity(3),
            new AnimalType(AnimalTypeValue.Default)
        );
        var animal = new Animal(
            new Name("Slinky"),
            new AnimalType(AnimalTypeValue.Default),
            DateTime.UtcNow.AddYears(-5),
            new Gender(GenderValue.Male),
            new Food(new Name("Insects"), new AnimalType(AnimalTypeValue.Default)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        enclosure.AddAnimal(animal);

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => enclosure.AddAnimal(animal));
        Assert.Equal("Animal is already in this enclosure!", ex.Message);
    }

    [Fact]
    public void RemoveAnimal_Succeeds_WhenPresent()
    {
        // Arrange
        var enclosure = new Enclosure(
            new Name("Big Cat House"),
            new Capacity(2),
            new AnimalType(AnimalTypeValue.Predator)
        );
        var animal = new Animal(
            new Name("Leo"),
            new AnimalType(AnimalTypeValue.Predator),
            DateTime.UtcNow.AddYears(-4),
            new Gender(GenderValue.Male),
            new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        enclosure.AddAnimal(animal);

        // Act
        enclosure.RemoveAnimal(animal.Id);

        // Assert
        Assert.Empty(enclosure.AnimalIds);
        Assert.Equal(0, enclosure.CurrentAnimalCount);
    }

    [Fact]
    public void RemoveAnimal_ThrowsInvalidOperationException_WhenNotPresent()
    {
        // Arrange
        var enclosure = new Enclosure(
            new Name("Aquarium"),
            new Capacity(5),
            new AnimalType(AnimalTypeValue.Fishes)
        );
        var randomId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => enclosure.RemoveAnimal(randomId));
        Assert.Equal("Animal is not in this enclosure!", ex.Message);
    }

    [Fact]
    public void Clean_UpdatesLastCleanedTimestamp()
    {
        // Arrange
        var enclosure = new Enclosure(
            new Name("Reptile House"),
            new Capacity(2),
            new AnimalType(AnimalTypeValue.Default)
        );
        var before = enclosure.LastCleaned;

        // Act
        enclosure.Clean();

        // Assert
        Assert.True(enclosure.LastCleaned > before);
    }
}