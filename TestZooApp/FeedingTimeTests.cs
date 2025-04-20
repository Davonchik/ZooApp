using ZooApplication.Domain.ValueObjects;

namespace TestZooApp;

public class FeedingTimeTests
{
    [Fact]
    public void Ctor_PastDate_Throws()
    {
        // Arrange / Act / Assert
        Assert.Throws<ArgumentException>(() =>
            new FeedingTime(DateTime.UtcNow.AddSeconds(-1)));
    }

    [Fact]
    public void Equals_AndToString_Work()
    {
        // Arrange
        var date = DateTime.UtcNow.AddMinutes(5);
        var f1 = new FeedingTime(date);
        var f2 = new FeedingTime(date);

        // Act & Assert
        Assert.Equal(f1, f2);
        Assert.Contains(date.ToString("o"), f1.ToString());
    }
}