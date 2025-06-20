using ZooApplication.Domain.Common;

namespace ZooApplication.Domain.ValueObjects;

/// <summary>
/// Gender Value Object.
/// </summary>
public class Gender : IEquatable<Gender>
{
    public GenderValue Value { get; private set; }

    public Gender(GenderValue value)
    {
        Value = value;
    }

    public bool Equals(Gender? other)
    {
        if (other == null) return false;
        return Value == other.Value;
    }

    public override bool Equals(object obj) => Equals(obj as Gender);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}