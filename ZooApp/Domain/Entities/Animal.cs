using ZooApp.Domain.Events;
using ZooApp.Domain.ValueObjects;
using ZooApp.Domain.ValueObjects;

namespace ZooApp.Domain.Entities;

public class Animal
{
    public Guid Id { get; private set; }
    public AnimalName Name { get; private set; }
    public string Species { get; private set; }
    public Guid? EnclosureId { get; private set; }
    public List<IDomainEvent> DomainEvents { get; private set; } = new List<IDomainEvent>();

    public Animal(AnimalName name, string species)
    {
        Id = Guid.NewGuid();
        Name = name;
        Species = species;
    }

    public void MoveToEnclosure(Guid enclosureId)
    {
        if (EnclosureId == enclosureId)
        {
            throw new InvalidOperationException("Animal is already in enclosure!");
        }
        var oldEnclosure = EnclosureId;
        EnclosureId = enclosureId;
        DomainEvents.Add(new AnimalMovedEvent(this, oldEnclosure, enclosureId));
    }
}