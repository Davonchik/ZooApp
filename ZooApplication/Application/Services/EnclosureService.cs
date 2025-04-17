using ZooApplication.Application.Interfaces;

namespace ZooApplication.Application.Services;

public class EnclosureService : IEnclosureService
{
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IAnimalService _animalService;
    
    public void DeleteEnclosure(Guid enclosureId)
    {
        var enclosure = _enclosureRepository.GetById(enclosureId);

        if (enclosure != null)
        {
            var animals = enclosure.AnimalIds;
            foreach (var animal in animals)
            {
                _animalService.DeleteAnimal(animal);
            }
            _enclosureRepository.Remove(enclosure);
        }
        else
        {
            throw new Exception("Enclosure not found!");
        }
    }
}