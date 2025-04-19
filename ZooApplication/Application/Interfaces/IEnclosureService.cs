using ZooApplication.Domain.Entities;

namespace ZooApplication.Application.Interfaces;

/// <summary>
/// Interface of Enclosure Service.
/// </summary>
public interface IEnclosureService
{
    IEnumerable<Enclosure> GetAll();

    Enclosure GetById(Guid id);

    Enclosure CreateEnclosure(Enclosure enclosure);

    Enclosure UpdateEnclosure(Guid id, Enclosure updatedModel);
    
    void DeleteEnclosure(Guid enclosureId);

    Enclosure CleanEnclosure(Guid id);
}