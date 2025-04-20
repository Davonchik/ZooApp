using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class AnimalEntityTests
{
    [Fact]
    public void Heal_WhenAlreadyHealthy_ReturnsFalseAndKeepsHealthy()
    {
        // Arrange
        var animal = new Animal(
            new Name("Test"),
            new AnimalType(AnimalTypeValue.Default),
            DateTime.UtcNow.AddYears(-1),
            new Gender(GenderValue.Male),
            new Food(new Name("Pellets"), new AnimalType(AnimalTypeValue.Default)),
            new HealthStatus(HealthStatusValue.Healthy)
        );

        // Act
        var result = animal.Heal();

        // Assert
        Assert.False(result);
        Assert.Equal(HealthStatusValue.Healthy, animal.HealthStatus.Value);
    }
}