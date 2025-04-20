using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Infrastructure.Repositories;

namespace TestZooApp;

public class InMemoryEnclosureRepositoryTests
{
    private readonly InMemoryEnclosureRepository _repo;

    public InMemoryEnclosureRepositoryTests()
    {
        _repo = new InMemoryEnclosureRepository();
    }

    [Fact]
    public void AddAndGetAll_ReturnsAddedEnclosure()
    {
        // Arrange
        var e = new Enclosure(new Name("E1"), new Capacity(3), new AnimalType(AnimalTypeValue.Default));

        // Act
        _repo.Add(e);
        var all = _repo.GetAll();

        // Assert
        Assert.Contains(e, all);
    }

    [Fact]
    public void GetById_Existing_ReturnsEnclosure()
    {
        // Arrange
        var e = new Enclosure(new Name("E2"), new Capacity(4), new AnimalType(AnimalTypeValue.Default));
        _repo.Add(e);

        // Act
        var fetched = _repo.GetById(e.Id);

        // Assert
        Assert.Same(e, fetched);
    }

    [Fact]
    public void GetById_NonExisting_ThrowsKeyNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<KeyNotFoundException>(() => _repo.GetById(id));
        Assert.Equal("Enclosure not found", ex.Message);
    }

    [Fact]
    public void Remove_RemovesEnclosure()
    {
        // Arrange
        var e = new Enclosure(new Name("E3"), new Capacity(2), new AnimalType(AnimalTypeValue.Default));
        _repo.Add(e);

        // Act
        _repo.Remove(e);
        var all = _repo.GetAll();

        // Assert
        Assert.DoesNotContain(e, all);
    }

    [Fact]
    public void Update_ValidKeepsIdAndReplaces()
    {
        // Arrange
        var original = new Enclosure(new Name("Orig"), new Capacity(2), new AnimalType(AnimalTypeValue.Default));
        _repo.Add(original);
        var updated = new Enclosure(new Name("Upd"), new Capacity(5), new AnimalType(AnimalTypeValue.Default));

        // Act
        _repo.Update(updated, original.Id);
        var fetched = _repo.GetById(original.Id);

        // Assert
        Assert.Equal(original.Id, fetched.Id);
        Assert.Equal("Upd", fetched.Name.Value);
        Assert.Equal(5, fetched.MaximumCapacity.Value);
    }

    [Fact]
    public void Update_NonExisting_ThrowsArgumentException()
    {
        // Arrange
        var updated = new Enclosure(new Name("U"), new Capacity(1), new AnimalType(AnimalTypeValue.Default));
        var id = Guid.NewGuid();

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _repo.Update(updated, id));
        Assert.Contains($"Enclosure with id: {id} does not exist!", ex.Message);
    }

    [Fact]
    public void Update_ShrinkBelowCurrentAnimalCount_ThrowsArgumentException()
    {
        var enclosure = new Enclosure(
            new Name("Bird Zone"),
            new Capacity(3),
            new AnimalType(AnimalTypeValue.Birds)
        );

        var bird1 = new Animal(
            new Name("B1"),
            new AnimalType(AnimalTypeValue.Birds),
            DateTime.UtcNow.AddYears(-1),
            new Gender(GenderValue.Male),
            new Food(new Name("Seed"), new AnimalType(AnimalTypeValue.Birds)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        var bird2 = new Animal(
            new Name("B2"),
            new AnimalType(AnimalTypeValue.Birds),
            DateTime.UtcNow.AddYears(-1),
            new Gender(GenderValue.Female),
            new Food(new Name("Seed"), new AnimalType(AnimalTypeValue.Birds)),
            new HealthStatus(HealthStatusValue.Healthy)
        );
        enclosure.AddAnimal(bird1);
        enclosure.AddAnimal(bird2);

        _repo.Add(enclosure);

        var updated = new Enclosure(
            new Name("Bird Zone"),
            new Capacity(1),
            new AnimalType(AnimalTypeValue.Birds)
        );

        var ex = Assert.Throws<ArgumentException>(() => _repo.Update(updated, enclosure.Id));
        Assert.Equal("Cannot make it so small - there are to many animals!", ex.Message);
    }

    [Fact]
    public void Update_ChangeTypeWithExistingAnimals_ThrowsArgumentException()
    {
        // Arrange
        var e = new Enclosure(new Name("E5"), new Capacity(2), new AnimalType(AnimalTypeValue.Predator));
        var animal = new Animal(new Name("P1"), new AnimalType(AnimalTypeValue.Predator),
                                DateTime.UtcNow, new Gender(GenderValue.Male),
                                new Food(new Name("Meat"), new AnimalType(AnimalTypeValue.Predator)),
                                new HealthStatus(HealthStatusValue.Healthy));
        e.AddAnimal(animal);
        _repo.Add(e);

        var updated = new Enclosure(new Name("E5"), new Capacity(3), new AnimalType(AnimalTypeValue.Herbivores));

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => _repo.Update(updated, e.Id));
        Assert.Equal("There are already animals with another type!", ex.Message);
    }

    [Fact]
    public void Update_ChangeTypeNoAnimals_Succeeds()
    {
        // Arrange
        var e = new Enclosure(new Name("E6"), new Capacity(2), new AnimalType(AnimalTypeValue.Predator));
        _repo.Add(e);

        var updated = new Enclosure(new Name("NewE6"), new Capacity(2), new AnimalType(AnimalTypeValue.Herbivores));

        // Act
        _repo.Update(updated, e.Id);
        var fetched = _repo.GetById(e.Id);

        // Assert
        Assert.Equal(AnimalTypeValue.Herbivores, fetched.EnclosureType.Value);
        Assert.Empty(fetched.AnimalIds);
    }
}