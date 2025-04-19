using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Services;

/// <summary>
/// Enclosure Service.
/// </summary>
public class EnclosureService : IEnclosureService
{
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IAnimalService _animalService;
    
    public EnclosureService(IEnclosureRepository enclosureRepository, IAnimalService animalService)
    {
        _enclosureRepository = enclosureRepository;
        _animalService = animalService;
    }

    /// <summary>
    /// Method for getting all enclosures.
    /// </summary>
    /// <returns>IEnumerable of enclosures.</returns>
    public IEnumerable<Enclosure> GetAll()
    {
        return _enclosureRepository.GetAll();
    }

    /// <summary>
    /// Method for getting enclosures by ID.
    /// </summary>
    /// <param name="id">Enclosure's ID.</param>
    /// <returns>Enclosure.</returns>
    public Enclosure GetById(Guid id)
    {
        return _enclosureRepository.GetById(id);
    }

    /// <summary>
    /// Method for enclosure creation.
    /// </summary>
    /// <param name="enclosure">Enclosure.</param>
    /// <returns>Enclosure.</returns>
    public Enclosure CreateEnclosure(Enclosure enclosure)
    {
        _enclosureRepository.Add(enclosure);
        return enclosure;
    }
    
    /// <summary>
    /// Method for updating info about enclosure.
    /// </summary>
    /// <param name="id">Enclosure's ID.</param>
    /// <param name="updatedModel">New Enclosure.</param>
    /// <returns>Enclosure.</returns>
    public Enclosure UpdateEnclosure(Guid id, Enclosure updatedModel)
    {
        var animalsIds = _enclosureRepository.GetById(id).AnimalIds.ToList();
            
        _enclosureRepository.Update(updatedModel, id);
            
        var newEnclosure = _enclosureRepository.GetById(id);
            
        foreach (var animalId in animalsIds)
        {
            newEnclosure.AddAnimal(_animalService.GetById(animalId));
        }
            
        return _enclosureRepository.GetById(id);
    }
    
    /// <summary>
    /// Method for enclosure deletion.
    /// </summary>
    /// <param name="enclosureId">Enclosure's ID.</param>
    /// <exception cref="Exception">Exception.</exception>
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
    
    /// <summary>
    /// Method for cleaning.
    /// </summary>
    /// <param name="id">Enclosure's ID.</param>
    /// <returns>Enclosure.</returns>
    public Enclosure CleanEnclosure(Guid id)
    {
        var enclosure = _enclosureRepository.GetById(id);
        enclosure.Clean();
        _enclosureRepository.Update(enclosure, id);
        return enclosure;
    }
}