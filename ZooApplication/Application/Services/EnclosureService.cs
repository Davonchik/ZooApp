using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Services;

public class EnclosureService : IEnclosureService
{
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IAnimalService _animalService;
    
    public EnclosureService(IEnclosureRepository enclosureRepository, IAnimalService animalService)
    {
        _enclosureRepository = enclosureRepository;
        _animalService = animalService;
    }

    public IEnumerable<Enclosure> GetAll()
    {
        return _enclosureRepository.GetAll();
    }

    public Enclosure GetById(Guid id)
    {
        return _enclosureRepository.GetById(id);
    }

    public Enclosure CreateEnclosure(Enclosure enclosure)
    {
        _enclosureRepository.Add(enclosure);
        return enclosure;
    }
    
    public Enclosure UpdateEnclosure(Guid id, Enclosure updatedModel)
    {
        var before = _enclosureRepository.GetById(id);
        _enclosureRepository.Update(updatedModel, id);
        return _enclosureRepository.GetById(id);
    }
    
    public void DeleteEnclosure(Guid enclosureId)
    {
        var enclosure = _enclosureRepository.GetById(enclosureId);
        if (enclosure == null)
            throw new Exception("Enclosure not found!");
        
        var animalsCopy = enclosure.AnimalIds.ToList();

        foreach (var animalId in animalsCopy)
        {
            _animalService.DeleteAnimal(animalId);
        }

        _enclosureRepository.Remove(enclosure);
    }
    
    public Enclosure CleanEnclosure(Guid id)
    {
        var enclosure = _enclosureRepository.GetById(id);
        enclosure.Clean();
        _enclosureRepository.Update(enclosure, id);
        return enclosure;
    }
}