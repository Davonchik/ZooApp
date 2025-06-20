using Moq;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class EnclosureServiceTests
{
    private readonly Mock<IEnclosureRepository> _encRepo;
    private readonly Mock<IAnimalService> _animalService;
    private readonly EnclosureService _svc;

    public EnclosureServiceTests()
    {
        _encRepo = new Mock<IEnclosureRepository>();
        _animalService = new Mock<IAnimalService>();
        _svc = new EnclosureService(_encRepo.Object, _animalService.Object);
    }

    [Fact]
    public void GetAll_ReturnsAllFromRepository()
    {
        // Arrange
        var list = new[]
        {
            new Enclosure(new Name("E1"), new Capacity(3), new AnimalType(AnimalTypeValue.Default)),
            new Enclosure(new Name("E2"), new Capacity(5), new AnimalType(AnimalTypeValue.Default))
        };
        _encRepo.Setup(r => r.GetAll()).Returns(list);

        // Act
        var result = _svc.GetAll();

        // Assert
        Assert.Same(list, result);
        _encRepo.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetById_ReturnsFromRepository()
    {
        // Arrange
        var id = Guid.NewGuid();
        var enclosure = new Enclosure(new Name("E1"), new Capacity(4), new AnimalType(AnimalTypeValue.Default))
        {
            Id = id
        };
        _encRepo.Setup(r => r.GetById(id)).Returns(enclosure);

        // Act
        var result = _svc.GetById(id);

        // Assert
        Assert.Equal(enclosure, result);
        _encRepo.Verify(r => r.GetById(id), Times.Once);
    }

    [Fact]
    public void CreateEnclosure_CallsAddAndReturnsEnclosure()
    {
        // Arrange
        var enclosure = new Enclosure(new Name("New"), new Capacity(2), new AnimalType(AnimalTypeValue.Default));

        // Act
        var result = _svc.CreateEnclosure(enclosure);

        // Assert
        Assert.Equal(enclosure, result);
        _encRepo.Verify(r => r.Add(enclosure), Times.Once);
    }

    [Fact]
    public void UpdateEnclosure_ReAddsExistingAnimals()
    {
        // Arrange
        var id = Guid.NewGuid();

        var a1 = new Animal(new Name("A1"), new AnimalType(AnimalTypeValue.Default), DateTime.UtcNow,
                            new Gender(GenderValue.Male), new Food(new Name("F"), new AnimalType(AnimalTypeValue.Default)),
                            new HealthStatus(HealthStatusValue.Healthy));
        var a2 = new Animal(new Name("A2"), new AnimalType(AnimalTypeValue.Default), DateTime.UtcNow,
                            new Gender(GenderValue.Female), new Food(new Name("G"), new AnimalType(AnimalTypeValue.Default)),
                            new HealthStatus(HealthStatusValue.Healthy));

        var original = new Enclosure(new Name("Orig"), new Capacity(5), new AnimalType(AnimalTypeValue.Default))
        {
            Id = id
        };
        original.AddAnimal(a1);
        original.AddAnimal(a2);

        var updated = new Enclosure(new Name("Upd"), new Capacity(10), new AnimalType(AnimalTypeValue.Default))
        {
            Id = id
        };

        _encRepo.SetupSequence(r => r.GetById(id))
                .Returns(original)
                .Returns(updated)
                .Returns(updated);

        _encRepo.Setup(r => r.Update(updated, id));
        _animalService.Setup(s => s.GetById(a1.Id)).Returns(a1);
        _animalService.Setup(s => s.GetById(a2.Id)).Returns(a2);

        // Act
        var result = _svc.UpdateEnclosure(id, updated);

        // Assert
        _encRepo.Verify(r => r.Update(updated, id), Times.Once);
        _encRepo.Verify(r => r.GetById(id), Times.Exactly(3));

        Assert.Contains(a1.Id, result.AnimalIds);
        Assert.Contains(a2.Id, result.AnimalIds);

        _animalService.Verify(s => s.GetById(a1.Id), Times.Once);
        _animalService.Verify(s => s.GetById(a2.Id), Times.Once);
    }

    [Fact]
    public void DeleteEnclosure_ExistingEnclosure_CallsDeleteAndRemove()
    {
        // Arrange
        var id = Guid.NewGuid();
        var e = new Enclosure(new Name("ToDelete"), new Capacity(3), new AnimalType(AnimalTypeValue.Default))
        {
            Id = id
        };
        var a1 = Guid.NewGuid();
        var a2 = Guid.NewGuid();
        // simulate AnimalIds
        typeof(Enclosure)
            .GetField("_animalIds", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .GetValue(e)
            .As<List<Guid>>()
            .AddRange(new[] { a1, a2 });

        _encRepo.Setup(r => r.GetById(id)).Returns(e);

        // Act
        _svc.DeleteEnclosure(id);

        // Assert
        _animalService.Verify(s => s.DeleteAnimal(a1), Times.Once);
        _animalService.Verify(s => s.DeleteAnimal(a2), Times.Once);
        _encRepo.Verify(r => r.Remove(e), Times.Once);
    }

    [Fact]
    public void DeleteEnclosure_NonExisting_Throws()
    {
        // Arrange
        var id = Guid.NewGuid();
        _encRepo.Setup(r => r.GetById(id)).Returns((Enclosure)null);

        // Act & Assert
        var ex = Assert.Throws<Exception>(() => _svc.DeleteEnclosure(id));
        Assert.Equal("Enclosure not found!", ex.Message);
    }

    [Fact]
    public void CleanEnclosure_CleansAndUpdates()
    {
        // Arrange
        var id = Guid.NewGuid();
        var e = new Enclosure(new Name("CleanMe"), new Capacity(1), new AnimalType(AnimalTypeValue.Default))
        {
            Id = id
        };
        var before = e.LastCleaned;
        _encRepo.Setup(r => r.GetById(id)).Returns(e);

        // Act
        var result = _svc.CleanEnclosure(id);

        // Assert
        Assert.Equal(e, result);
        Assert.True(result.LastCleaned > before);
        _encRepo.Verify(r => r.Update(e, id), Times.Once);
    }
}

static class ReflectionExtensions
{
    public static T As<T>(this object obj) => (T)obj;
}