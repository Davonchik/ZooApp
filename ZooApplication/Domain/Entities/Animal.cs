using ZooApplication.Application.Handlers;
using ZooApplication.Domain.Events;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Domain.Common;

namespace ZooApplication.Domain.Entities;

/// <summary>
/// Animal entity.
/// </summary>
public class Animal : EventEntity
{
    public Guid Id { get; set; }

    public Name Name { get; private set; }

    public AnimalType Species { get; set; }

    public DateTime BirthDate { get; private set; }

    public Gender Gender { get; private set; }

    public Food FavoriteFood { get; private set; }

    public HealthStatus HealthStatus { get; private set; }

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
    public bool Feed()
    {
        return true;
    }

    /// <summary>
    /// Healing animal method.
    /// </summary>
    public bool Heal()
    {
        if (HealthStatus.Value == HealthStatusValue.Sick)
        {
            HealthStatus = new HealthStatus(HealthStatusValue.Healthy);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Move Event Method.
    /// </summary>
    /// <param name="animal">Animal.</param>
    /// <param name="oldEnclosureId">Old Enclosure's ID.</param>
    /// <param name="newEnclosureId">New Enclosure's ID.</param>
    internal void AddMoveToEnclosureEvent(Animal animal, Guid oldEnclosureId, Guid newEnclosureId)
    {
        AddDomainEvent(new AnimalMovedEvent(animal, oldEnclosureId, newEnclosureId));
    }
}