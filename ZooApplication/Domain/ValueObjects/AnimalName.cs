namespace ZooApplication.Domain.ValueObjects;

public class AnimalName : IEquatable<AnimalName>
{
    public string Value { get; private set; }

    public AnimalName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Animal`s name cannot be null or whitespace.", nameof(value));
        }
        Value = value;
    }

    public bool Equals(AnimalName otherName)
    {
        if (otherName == null) return false;
        return Value == otherName.Value;
    }
    
    public override bool Equals(object obj) => Equals(obj as AnimalName);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}