using ZooApplication.Domain.Common;
using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class HealthStatusTests
{
    [Fact]
    public void Equals_AndHashCode_Work()
    {
        // Arrange
        var s1 = new HealthStatus(HealthStatusValue.Healthy);
        var s2 = new HealthStatus(HealthStatusValue.Healthy);
        var s3 = new HealthStatus(HealthStatusValue.Sick);

        // Act & Assert
        Assert.Equal(s1, s2);
        Assert.Equal(s1.GetHashCode(), s2.GetHashCode());
        Assert.NotEqual(s1, s3);
    }
}