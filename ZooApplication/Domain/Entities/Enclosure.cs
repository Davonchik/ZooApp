using ZooApplication.Domain.Common;
using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Domain.Entities;

public class Enclosure
{
    private readonly List<Guid> _animalIds = [];
    public Guid Id { get; set; }
    public Name Name { get; private set; }
    public AnimalType EnclosureType { get; set; } 
    public Capacity MaximumCapacity { get; private set; }
    public DateTime LastCleaned { get; private set; } = DateTime.UtcNow;
    public IReadOnlyCollection<Guid> AnimalIds => _animalIds.AsReadOnly();
    public int CurrentAnimalCount => _animalIds.Count;

    public Enclosure(Name name, Capacity maximumCapacity, AnimalType enclosureType)
    {
        Id = Guid.NewGuid();
        Name = name;
        EnclosureType = enclosureType;
        MaximumCapacity = maximumCapacity;
    }

    /// <summary>
    /// Method for animal addition.
    /// </summary>
    /// <param name="animalId">Animal`s ID.</param>
    /// <exception cref="InvalidOperationException">Exception.</exception>
    public void AddAnimal(Animal animal)
    {
        if (animal.Species.Value != EnclosureType.Value)
        {
            throw new ArgumentException("Type mismatch!");
        }
        
        if (CurrentAnimalCount >= MaximumCapacity.Value)
        {
            throw new InvalidOperationException("Cannot add more animals to enclosure!");
        }

        if (_animalIds.Contains(animal.Id))
        {
            throw new InvalidOperationException("Animal is already in this enclosure!");
        }
        
        _animalIds.Add(animal.Id);
    }

    /// <summary>
    /// Method for animal removing.
    /// </summary>
    /// <param name="animalId">Animal`s ID.</param>
    /// <exception cref="InvalidOperationException">Exception.</exception>
    public void RemoveAnimal(Guid animalId)
    {
        if (!_animalIds.Contains(animalId))
        {
            throw new InvalidOperationException("Animal is not in this enclosure!");
        }
        _animalIds.Remove(animalId);
    }

    /// <summary>
    /// Method for enclosure cleaning.
    /// </summary>
    public void Clean()
    {
        LastCleaned = DateTime.UtcNow;
        Console.WriteLine($"Enclosure {Id} cleaned: {LastCleaned}.");
    }
}