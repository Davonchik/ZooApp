using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

/// <summary>
/// Interface of Animal Repository.
/// </summary>
public interface IAnimalRepository
{
    Animal GetById(Guid id);
    IEnumerable<Animal> GetAll();
    void Add(Animal animal);
    void Remove(Animal animal);
    void Update(Animal newAnimalModel, Guid animalId);
}