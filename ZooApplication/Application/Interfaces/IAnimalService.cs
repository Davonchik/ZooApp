using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

public interface IAnimalService
{
    IEnumerable<Animal> GetAll();
    
    Animal GetById(Guid id);

    bool CreateAnimal(Animal animal, Guid enclosureId);
    
    bool UpdateAnimal(Animal newAnimalModel, Guid animalId); 
    
    void Heal(Guid id);
    
    void Feed(Guid id);
    
    void DeleteAnimal(Guid id);
}