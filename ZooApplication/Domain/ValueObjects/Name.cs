namespace ZooApplication.Domain.ValueObjects;

/// <summary>
/// Name Value Object.
/// </summary>
public class Name : IEquatable<Name>
{
    public string Value { get; private set; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(value));
        }
        Value = value;
    }

    public bool Equals(Name otherName)
    {
        if (otherName == null) return false;
        return Value == otherName.Value;
    }
    
    public override bool Equals(object obj) => Equals(obj as Name);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}