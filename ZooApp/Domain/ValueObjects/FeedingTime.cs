namespace ZooApp.Domain.ValueObjects;

public class FeedingTime : IEquatable<FeedingTime>
{
    public DateTime Value { get; private set; }

    public FeedingTime(DateTime value)
    {
        if (value < DateTime.UtcNow)
        {
            throw new ArgumentException("Feeding time must be in the future.");
        }
        Value = value;
    }

    public bool Equals(FeedingTime other)
    {
        if (other == null) return false;
        return Value.Equals(other.Value);
    }
    
    public override bool Equals(object obj) => Equals(obj as FeedingTime);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString("o");
}