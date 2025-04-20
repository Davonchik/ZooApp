using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class CapacityTests
{
    [Fact]
    public void Ctor_Negative_Throws()
    {
        // Arrange / Act / Assert
        Assert.Throws<ArgumentException>(() => new Capacity(-1));
    }

    [Fact]
    public void Equals_AndHashCode_Work()
    {
        // Arrange
        var c1 = new Capacity(10);
        var c2 = new Capacity(10);
        var c3 = new Capacity(5);

        // Act & Assert
        Assert.Equal(c1, c2);
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
        Assert.NotEqual(c1, c3);
    }
}