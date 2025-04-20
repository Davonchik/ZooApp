using ZooApplication.Domain.Common;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class GenderTests
{
    [Fact]
    public void Equals_AndHashCode_Work()
    {
        // Arrange
        var g1 = new Gender(GenderValue.Male);
        var g2 = new Gender(GenderValue.Male);
        var g3 = new Gender(GenderValue.Female);

        // Act & Assert
        Assert.Equal(g1, g2);
        Assert.Equal(g1.GetHashCode(), g2.GetHashCode());
        Assert.NotEqual(g1, g3);
    }
}