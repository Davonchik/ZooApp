using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Infrastructure.Repositories;

public class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly List<Animal> _animals = new List<Animal>();
    
    public void Add(Animal animal) => _animals.Add(animal);
    
    public IEnumerable<Animal> GetAll() => _animals;
    
    public Animal GetById(Guid id) => _animals.FirstOrDefault(a => a.Id == id) ?? 
                                      throw new KeyNotFoundException("No animal was found");
    public void Remove(Animal animal) => _animals.Remove(animal);

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