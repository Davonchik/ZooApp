using ZooApp.Domain.Events;
using ZooApp.Domain.ValueObjects;
using ZooApp.Domain.ValueObjects;

namespace ZooApp.Domain.Entities;

public enum HealthStatus
{
    Healthy,
    Sick
}

public enum Gender
{
    Male,
    Female
}

public class Animal
{
    public Guid Id { get; private set; }
    
    public AnimalName Name { get; private set; }
    
    public string Species { get; private set; }
    
    public DateTime BirthDate { get; private set; }
    
    public Gender Gender { get; private set; }
    
    public string FavoriteFood { get; private set; }
    
    public HealthStatus HealthStatus { get; private set; }
    
    public List<IDomainEvent> DomainEvents { get; private set; } = new List<IDomainEvent>();

    public Animal(AnimalName name, string species, DateTime birthDate, Gender gender, string favoriteFood, 
        HealthStatus healthStatus = HealthStatus.Healthy)
    {
        Id = Guid.NewGuid();
        Name = name;
        Species = species;
        BirthDate = birthDate;
        Gender = gender;
        FavoriteFood = favoriteFood;
        HealthStatus = healthStatus;
    }

    /// <summary>
    /// Feeding animal method.
    /// </summary>
    public void Feed()
    {
        Console.WriteLine($"Animal {Name} is feeded.");
    }

    /// <summary>
    /// Healing animal method.
    /// </summary>
    public void Heal()
    {
        if (HealthStatus == HealthStatus.Sick)
        {
            HealthStatus = HealthStatus.Healthy;
            Console.WriteLine($"Animal {Name} is healed.");
        }
    }
}