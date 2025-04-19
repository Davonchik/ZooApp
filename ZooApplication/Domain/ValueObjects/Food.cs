using ZooApplication.Domain.Common;

namespace ZooApplication.Domain.ValueObjects;

/// <summary>
/// Food Value Object.
/// </summary>
public class Food : IEquatable<Food>
{
    public Name Name { get; set; }
    
    public AnimalType Match { get; set; }

    public Food(Name name, AnimalType animalType)
    {
        Name = name;
        Match = animalType;
    }
    
    public bool Equals(Food? other)
    {
        if (other is null) return false;

        if (other.Name.Equals(Name) && Match.Equals(other.Match)) return true;
        
        return false;
    }
    
    public override bool Equals(object obj) => Equals(obj as Food);
    
    public override int GetHashCode() => Name.GetHashCode() + Match.GetHashCode();
    
    public override string ToString() => $"Name: {Name}, Match: {Match}";
}