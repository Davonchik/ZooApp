namespace ZooApplication.Domain.ValueObjects;

/// <summary>
/// Capacity Value Object.
/// </summary>
public class Capacity : IEquatable<Capacity>
{
    public int Value { get; private set; }

    public Capacity(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Capacity cannot be negative!", nameof(value));
        }
        Value = value;
    }

    public bool Equals(Capacity otherCapacity)
    {
        if (otherCapacity == null) return false;
        return Value == otherCapacity.Value;
    }
    
    public override bool Equals(object obj) => Equals(obj as Capacity);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}