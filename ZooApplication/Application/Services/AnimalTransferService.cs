using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Services;

/// <summary>
/// Animal Transfer Service.
/// </summary>
public class AnimalTransferService : IAnimalTransferService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public AnimalTransferService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _domainEventDispatcher = domainEventDispatcher;
    }

    /// <summary>
    /// Method for animal moving.
    /// </summary>
    /// <param name="animalId">Animal's ID.</param>
    /// <param name="targetEnclosureId">Target Enclosure's ID.</param>
    /// <exception cref="ArgumentException">Exception.</exception>
    public void TransferAnimal(Guid animalId, Guid targetEnclosureId)
    {
        var animal = _animalRepository.GetById(animalId);
    
        var targetEnclosure = _enclosureRepository.GetById(targetEnclosureId);

        if (animal.Species.Value != targetEnclosure.EnclosureType.Value)
        {
            throw new ArgumentException("Type mismatch!");
        }
        var old = _enclosureRepository.GetAll().FirstOrDefault(x => x.AnimalIds.Contains(animalId));
    

        if (old != null && targetEnclosure.CurrentAnimalCount + 1 <= targetEnclosure.MaximumCapacity.Value)
        {
            old.RemoveAnimal(animal.Id);
            animal.AddMoveToEnclosureEvent(animal, old.Id, targetEnclosureId);
        }

        targetEnclosure.AddAnimal(animal);
        _domainEventDispatcher.Dispatch(animal.DomainEvents);
        animal.ClearDomainEvents();
    }
}