using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

public interface IEnclosureRepository
{
    Enclosure GetById(Guid id);
    IEnumerable<Enclosure> GetAll();
    void Add(Enclosure enclosure);
    void Remove(Enclosure enclosure);
    void Update(Enclosure enclosure);
}