using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IAnimalTransferService _transferService;

    public AnimalService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository,
        IAnimalTransferService transferService)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _transferService = transferService;
    }

    public IEnumerable<Animal> GetAll()
    {
        return _animalRepository.GetAll();
    }
    
    public Animal GetById(Guid id)
    {
        return _animalRepository.GetById(id);
    }

    public bool CreateAnimal(Animal animal, Guid enclosureId)
    {
        try
        {
            _animalRepository.Add(animal);
            _transferService.TransferAnimal(animal.Id, enclosureId);
        }
        catch (Exception ex)
        {
            _animalRepository.Remove(animal);
        }
        return true;
    }

    public bool UpdateAnimal(Animal newAnimalModel, Guid animalId)
    {
        _animalRepository.Update(newAnimalModel, animalId);
        return true;
    }
    
    public void Heal(Guid id)
    {
        var animal = _animalRepository.GetById(id);

        if (animal.HealthStatus.Value == HealthStatusValue.Healthy)
        {
            throw new Exception($"The animal {id} is already healed.");
        }
        
        animal.Heal();
    }

    public void Feed(Guid id)
    {
        var animal = _animalRepository.GetById(id);
        
        animal.Feed();
    }

    public void DeleteAnimal(Guid id)
    {
        var animal = _animalRepository.GetById(id);
        var enclosure = _enclosureRepository.GetAll().FirstOrDefault(enclosure => enclosure.AnimalIds.Contains(id));
        
        if (enclosure != null)
            enclosure.RemoveAnimal(id);
        
        _animalRepository.Remove(animal);
    }
}