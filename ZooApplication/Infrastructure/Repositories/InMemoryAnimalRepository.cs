using ZooApp.Application.Interfaces;
using ZooApp.Domain.Entities;

namespace ZooApp.Infrastructure.Repositories;

public class InMemoryAnimalRepository : IAnimalRepository
{
    private readonly List<Animal> _animals = new List<Animal>();
    public void Add(Animal animal) => _animals.Add(animal);
    public IEnumerable<Animal> GetAll() => _animals;
    public Animal GetById(Guid id) => _animals.FirstOrDefault(a => a.Id == id) ?? 
                                      throw new KeyNotFoundException("No animal was found");
    public void Remove(Animal animal) => _animals.Remove(animal);
    public void Update(Animal animal) {}
}