using ZooApplication.Domain.Common;

namespace ZooApplication.Domain.ValueObjects;

/// <summary>
/// AnimalType Value Object.
/// </summary>
public class AnimalType : IEquatable<AnimalType>
{
    public AnimalTypeValue Value { get; private set; }

    public AnimalType(AnimalTypeValue value)
    {
        Value = value;
    }

    public bool Equals(AnimalType? other)
    {
        if (other == null) return false;
        return Value == other.Value;
    }
    
    public override bool Equals(object obj) => Equals(obj as AnimalType);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}