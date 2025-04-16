namespace ZooApplication.Domain.Entities;

public class Enclosure
{
    private readonly List<Guid> _animalIds = [];
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string EnclosureType { get; private set; } 
    public double Size { get; private set; }
    public int MaximumCapacity { get; private set; }
    public DateTime LastCleaned { get; private set; } = DateTime.MinValue;
    public IReadOnlyCollection<Guid> AnimalIds => _animalIds.AsReadOnly();
    public int CurrentAnimalCount => _animalIds.Count;

    public Enclosure(string enclosureType, double size, int maximumCapacity)
    {
        Id = Guid.NewGuid();
        EnclosureType = enclosureType;
        Size = size;
        MaximumCapacity = maximumCapacity;
    }

    /// <summary>
    /// Method for animal addition.
    /// </summary>
    /// <param name="animalId">Animal`s ID.</param>
    /// <exception cref="InvalidOperationException">Exception.</exception>
    public void AddAnimal(Guid animalId)
    {
        if (CurrentAnimalCount >= MaximumCapacity)
        {
            throw new InvalidOperationException("Cannot add more animals to enclosure!");
        }

        if (_animalIds.Contains(animalId))
        {
            throw new InvalidOperationException("Animal is already in this enclosure!");
        }
        
        _animalIds.Add(animalId);
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

    public void ChangeName(string name)
    {
        Name = name;
    }
}