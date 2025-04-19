using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Services;

/// <summary>
/// Animal Service.
/// </summary>
public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IAnimalTransferService _transferService;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public AnimalService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository,
        IAnimalTransferService transferService, IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _transferService = transferService;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    /// <summary>
    /// Method for getting all animals.
    /// </summary>
    /// <returns>IEnumerable of animals.</returns>
    public IEnumerable<Animal> GetAll()
    {
        return _animalRepository.GetAll();
    }
    
    /// <summary>
    /// Method for getting animal by ID.
    /// </summary>
    /// <param name="id">Animal's ID.</param>
    /// <returns>Animal.</returns>
    public Animal GetById(Guid id)
    {
        return _animalRepository.GetById(id);
    }

    /// <summary>
    /// Nethod for animal creation.
    /// </summary>
    /// <param name="animal">Animal.</param>
    /// <param name="enclosureId">Enclosure's ID.</param>
    /// <returns>Bool value.</returns>
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
            throw;
        }
        return true;
    }

    /// <summary>
    /// Method for Animal's info updating.
    /// </summary>
    /// <param name="newAnimalModel">New Animal's info.</param>
    /// <param name="animalId">Animal's ID.</param>
    /// <returns>Bool value.</returns>
    public bool UpdateAnimal(Animal newAnimalModel, Guid animalId)
    {
        _animalRepository.Update(newAnimalModel, animalId);
        return true;
    }
    
    /// <summary>
    /// Method for healing animal.
    /// </summary>
    /// <param name="id">Animal's ID.</param>
    /// <exception cref="Exception">Exception.</exception>
    public void Heal(Guid id)
    {
        var animal = _animalRepository.GetById(id);

        if (animal.HealthStatus.Value == HealthStatusValue.Healthy)
        {
            throw new Exception($"The animal {id} is already healed.");
        }
        
        animal.Heal();
    }

    /// <summary>
    /// Method for feeding animal.
    /// </summary>
    /// <param name="id">Animal's ID.</param>
    public void Feed(Guid id)
    {
        var animal = _animalRepository.GetById(id);
        
        animal.Feed();
    }

    /// <summary>
    /// Method for deletion animal.
    /// </summary>
    /// <param name="id">Animal's ID.</param>
    public void DeleteAnimal(Guid id)
    {
        var animal = _animalRepository.GetById(id);
        var enclosure = _enclosureRepository.GetAll().FirstOrDefault(enclosure => enclosure.AnimalIds.Contains(id));
        
        
        var currentSchedules = _feedingScheduleRepository.GetAll()
            .Where(schedule => schedule.AnimalId == id).ToList();
        
        if (enclosure != null)
            enclosure.RemoveAnimal(id);
        
        
        if (currentSchedules.Any())
        {
            foreach (var sch in currentSchedules)
            {
                _feedingScheduleRepository.Remove(sch);
            }
        }
        
        _animalRepository.Remove(animal);
    }
}