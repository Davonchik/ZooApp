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
    
        var targetEnclosure = _enclosureRepository.GetById(targetEnclosureId);

        var old = _enclosureRepository.GetAll().FirstOrDefault(x => x.AnimalIds.Contains(animalId));
    

        if (old != null)
        {
            old.RemoveAnimal(animal.Id);
            _enclosureRepository.Update(old);
        }

        targetEnclosure.AddAnimal(animal.Id);
    
        _enclosureRepository.Update(targetEnclosure);
    }
}