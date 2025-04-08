using ZooApp.Application.Interfaces;
using ZooApp.Domain.Entities;

namespace ZooApp.Application.Services;

public class AnimalTransferService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;

    public AnimalTransferService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
    }

    public void TransferAnimal(Guid animalId, Guid targetEnclosureId)
    {
        var animal = _animalRepository.GetById(animalId);
        if (animal == null)
        {
            throw new Exception("Animal not found!");
        }
        
        var targetEnclosure = _enclosureRepository.GetById(targetEnclosureId);
        if (targetEnclosure == null)
        {
            throw new Exception("Target enclosure not found!");
        }

        if (animal.EnclosureId.HasValue)
        {
            var oldEnclosure = _enclosureRepository.GetById(animal.EnclosureId.Value);
            if (oldEnclosure != null)
            {
                oldEnclosure.RemoveAnimal(animal.Id);
                _enclosureRepository.Update(oldEnclosure);
            }
        }
        
        targetEnclosure.AddAnimal(animal.Id);
        _enclosureRepository.Update(targetEnclosure);
        
        animal.MoveToEnclosure(targetEnclosureId);
        _animalRepository.Update(animal);
    }
}