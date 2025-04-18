using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Services;

public class AnimalTransferService : IAnimalTransferService
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
    
        var targetEnclosure = _enclosureRepository.GetById(targetEnclosureId);

        if (animal.Species.Value != targetEnclosure.EnclosureType.Value)
        {
            throw new ArgumentException("Type mismatch!");
        }
        var old = _enclosureRepository.GetAll().FirstOrDefault(x => x.AnimalIds.Contains(animalId));
    

        if (old != null && old.MaximumCapacity.Value + 1 < targetEnclosure.MaximumCapacity.Value)
        {
            old.RemoveAnimal(animal.Id);
        }

        targetEnclosure.AddAnimal(animal);
    }
}