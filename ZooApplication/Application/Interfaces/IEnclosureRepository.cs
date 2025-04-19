using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

/// <summary>
/// Interface of Enclosure Repository.
/// </summary>
public interface IEnclosureRepository
{
    Enclosure GetById(Guid id);
    IEnumerable<Enclosure> GetAll();
    void Add(Enclosure enclosure);
    void Remove(Enclosure enclosure);
    void Update(Enclosure newEnclosureModel, Guid enclosureId);
}