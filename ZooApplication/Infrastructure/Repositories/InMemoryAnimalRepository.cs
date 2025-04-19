using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Infrastructure.Repositories;

/// <summary>
/// In Memory Animal Repository.
/// </summary>
public class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly List<Animal> _animals = new List<Animal>();
    
    /// <summary>
    /// Method for animal addition to repository.
    /// </summary>
    /// <param name="animal">Animal.</param>
    public void Add(Animal animal) => _animals.Add(animal);
    
    /// <summary>
    /// Method for getting all animals.
    /// </summary>
    /// <returns>IEnumerable of animals.</returns>
    public IEnumerable<Animal> GetAll() => _animals;
    
    /// <summary>
    /// Method for getting animal by ID.
    /// </summary>
    /// <param name="id">Animal's ID.</param>
    /// <returns>Animal</returns>
    /// <exception cref="KeyNotFoundException">Exception.</exception>
    public Animal GetById(Guid id) => _animals.FirstOrDefault(a => a.Id == id) ?? 
                                      throw new KeyNotFoundException("No animal was found");
    
    /// <summary>
    /// Method for removing animal.
    /// </summary>
    /// <param name="animal">Animal.</param>
    public void Remove(Animal animal) => _animals.Remove(animal);

    /// <summary>
    /// Method for updating Animal's info.
    /// </summary>
    /// <param name="newAnimalModel">New Animal Info.</param>
    /// <param name="animalId">Animal's ID.</param>
    /// <exception cref="KeyNotFoundException">Exception.</exception>
    public void Update(Animal newAnimalModel, Guid animalId)
    {
        var existingAnimal = _animals.FirstOrDefault(a => a.Id == animalId);
        if (existingAnimal == null)
            throw new KeyNotFoundException($"No animal found with Id {animalId}");
        
        newAnimalModel.Id = existingAnimal.Id;
        newAnimalModel.Species = existingAnimal.Species;
        _animals.Remove(existingAnimal);
        _animals.Add(newAnimalModel);
    }
}