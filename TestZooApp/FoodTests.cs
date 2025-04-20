using ZooApplication.Domain.Common;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class FoodTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        // Arrange
        var name = new Name("Apple");
        var match = new AnimalType(AnimalTypeValue.Herbivores);

        // Act
        var food = new Food(name, match);

        // Assert
        Assert.Equal(name, food.Name);
        Assert.Equal(match, food.Match);
    }

    [Fact]
    public void Equals_SameValues_ReturnsTrue()
    {
        // Arrange
        var name1 = new Name("Berries");
        var type1 = new AnimalType(AnimalTypeValue.Herbivores);
        var food1 = new Food(name1, type1);
        var food2 = new Food(new Name("Berries"), new AnimalType(AnimalTypeValue.Herbivores));

        // Act & Assert
        Assert.True(food1.Equals(food2));
        Assert.True(food1.Equals((object)food2));
        Assert.Equal(food1.GetHashCode(), food2.GetHashCode());
    }

    [Fact]
    public void Equals_DifferentName_ReturnsFalse()
    {
        // Arrange
        var food1 = new Food(new Name("Grass"), new AnimalType(AnimalTypeValue.Herbivores));
        var food2 = new Food(new Name("Leaves"), new AnimalType(AnimalTypeValue.Herbivores));

        // Act & Assert
        Assert.False(food1.Equals(food2));
        Assert.False(food1.Equals((object)food2));
    }

    [Fact]
    public void Equals_DifferentMatch_ReturnsFalse()
    {
        // Arrange
        var food1 = new Food(new Name("Fish"), new AnimalType(AnimalTypeValue.Fishes));
        var food2 = new Food(new Name("Fish"), new AnimalType(AnimalTypeValue.Predator));

        // Act & Assert
        Assert.False(food1.Equals(food2));
    }

    [Fact]
    public void Equals_NullOrOtherType_ReturnsFalse()
    {
        // Arrange
        var food = new Food(new Name("Seed"), new AnimalType(AnimalTypeValue.Birds));

        // Act & Assert
        Assert.False(food.Equals(null));
        Assert.False(food.Equals("not a food"));
    }

    [Fact]
    public void GetHashCode_ConsistentWithEquals()
    {
        // Arrange
        var food1 = new Food(new Name("Worms"), new AnimalType(AnimalTypeValue.Birds));
        var food2 = new Food(new Name("Worms"), new AnimalType(AnimalTypeValue.Birds));

        // Act & Assert
        Assert.Equal(food1.GetHashCode(), food2.GetHashCode());
    }

    [Fact]
    public void ToString_IncludesNameAndMatch()
    {
        // Arrange
        var name = new Name("Pellets");
        var match = new AnimalType(AnimalTypeValue.Default);
        var food = new Food(name, match);

        // Act
        var str = food.ToString();

        // Assert
        Assert.Contains("Name: Pellets", str);
        Assert.Contains($"Match: {match}", str);
    }
}