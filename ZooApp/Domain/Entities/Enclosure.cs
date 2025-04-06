namespace ZooApp.Domain.Entities;

public class Enclosure
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Capacity { get; private set; }
    public List<Guid> AnimalsIds { get; private set; } = new List<Guid>();

    public Enclosure(string name, int capacity)
    {
        Id = Guid.NewGuid();
        Name = name;
        Capacity = capacity;
    }

    public void AddAnimal(Guid animalId)
    {
        if (AnimalsIds.Count >= Capacity)
        {
            throw new InvalidOperationException("Cannot add more animals to enclosure!");
        }

        if (AnimalsIds.Contains(animalId))
        {
            throw new InvalidOperationException("Animal is already in enclosure!");
        }
        
        AnimalsIds.Add(animalId);
    }

    public void RemoveAnimal(Guid animalId)
    {
        if (!AnimalsIds.Contains(animalId))
        {
            throw new InvalidOperationException("Animal is not in enclosure!");
        }
        AnimalsIds.Remove(animalId);
    }
}