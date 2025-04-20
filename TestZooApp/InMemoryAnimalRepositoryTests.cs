using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Infrastructure.Repositories;

namespace TestZooApp;

public class InMemoryAnimalRepositoryTests
{
    private readonly InMemoryAnimalRepository _repo = new();

    private static Animal MakeAnimal(string name, AnimalTypeValue type) =>
        new(
            new Name(name),
            new AnimalType(type),
            DateTime.UtcNow.AddYears(-1),
            new Gender(GenderValue.Male),
            new Food(new Name("Food"), new AnimalType(type)),
            new HealthStatus(HealthStatusValue.Healthy)
        );

    [Fact]
    public void AddAndGetAll_ReturnsAddedAnimal()
    {
        // Arrange
        var animal = MakeAnimal("A1", AnimalTypeValue.Default);

        // Act
        _repo.Add(animal);
        var result = _repo.GetAll();

        // Assert
        Assert.Contains(animal, result);
    }

    [Fact]
    public void GetById_Existing_ReturnsAnimal()
    {
        // Arrange
        var animal = MakeAnimal("A2", AnimalTypeValue.Predator);
        _repo.Add(animal);

        // Act
        var fetched = _repo.GetById(animal.Id);

        // Assert
        Assert.Same(animal, fetched);
    }

    [Fact]
    public void GetById_NonExisting_ThrowsKeyNotFound()
    {
        // Arrange
        var missingId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<KeyNotFoundException>(() => _repo.GetById(missingId));
        Assert.Equal("No animal was found", ex.Message);
    }

    [Fact]
    public void Remove_DeletesAnimal()
    {
        // Arrange
        var animal = MakeAnimal("A3", AnimalTypeValue.Fishes);
        _repo.Add(animal);

        // Act
        _repo.Remove(animal);

        // Assert
        Assert.DoesNotContain(animal, _repo.GetAll());
    }

    [Fact]
    public void Update_ReplacesAnimal_AndKeepsIdAndSpecies()
    {
        // Arrange
        var original = MakeAnimal("Orig", AnimalTypeValue.Birds);
        _repo.Add(original);

        var updated = new Animal(
            new Name("Updated"),
            new AnimalType(AnimalTypeValue.Predator),
            original.BirthDate,
            new Gender(GenderValue.Female),
            new Food(new Name("Seeds"), new AnimalType(AnimalTypeValue.Predator)),
            new HealthStatus(HealthStatusValue.Sick)
        );

        // Act
        _repo.Update(updated, original.Id);
        var fetched = _repo.GetById(original.Id);

        // Assert
        Assert.Equal(original.Id, fetched.Id);
        Assert.Equal("Updated", fetched.Name.Value);
        Assert.Equal(AnimalTypeValue.Birds, fetched.Species.Value);
    }

    [Fact]
    public void Update_NonExisting_ThrowsKeyNotFound()
    {
        // Arrange
        var model = MakeAnimal("Ghost", AnimalTypeValue.Default);
        var nonExistingId = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<KeyNotFoundException>(() => _repo.Update(model, nonExistingId));
        Assert.Equal($"No animal found with Id {nonExistingId}", ex.Message);
    }
}