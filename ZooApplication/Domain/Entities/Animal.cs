using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Domain.Common;

namespace ZooApplication.Domain.Entities;

public class Animal
{
    public Guid Id { get; set; }
    
    public Name Name { get; private set; }
    
    public AnimalType Species { get; set; }
    
    public DateTime BirthDate { get; private set; }
    
    public Gender Gender { get; private set; }
    
    public Food FavoriteFood { get; private set; }
    
    public HealthStatus HealthStatus { get; private set; }
    
    public List<IDomainEvent> DomainEvents { get; private set; } = new List<IDomainEvent>();

    public Animal(Name name, AnimalType species, DateTime birthDate, Gender gender, Food favoriteFood, 
        HealthStatus healthStatusValue)
    {
        Id = Guid.NewGuid();
        Name = name;
        Species = species;
        BirthDate = birthDate;
        Gender = gender;
        FavoriteFood = favoriteFood;
        HealthStatus = healthStatusValue;
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
        if (HealthStatus.Value == HealthStatusValue.Sick)
        {
            HealthStatus = new HealthStatus(HealthStatusValue.Healthy);
            Console.WriteLine($"Animal {Name} is healed.");
        }
    }
}