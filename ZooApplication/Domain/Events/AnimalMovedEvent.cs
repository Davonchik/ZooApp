using ZooApplication.Domain.Entities;

namespace ZooApplication.Domain.Events;

/// <summary>
/// Animal Moved Event.
/// </summary>
public class AnimalMovedEvent : IDomainEvent
{
    public Animal Animal { get; private set; }
    public Guid? OldEnclosureId { get; private set; }
    public Guid NewEnclosureId { get; private set; }
    public DateTime OccurredOn { get; private set; }

    public AnimalMovedEvent(Animal animal, Guid? oldEnclosureId, Guid newEnclosureId)
    {
        Animal = animal;
        OldEnclosureId = oldEnclosureId;
        NewEnclosureId = newEnclosureId;
        OccurredOn = DateTime.UtcNow;
    }
}