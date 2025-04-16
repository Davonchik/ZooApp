using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

public interface IAnimalRepository
{
    Animal GetById(Guid id);
    IEnumerable<Animal> GetAll();
    void Add(Animal animal);
    void Remove(Animal animal);
    void Update(Animal animal);
}