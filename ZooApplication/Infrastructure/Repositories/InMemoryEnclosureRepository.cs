using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Infrastructure.Repositories;

public class InMemoryEnclosureRepository : IEnclosureRepository
{
    private readonly List<Enclosure> _enclosures = new List<Enclosure>();
    
    public void Add(Enclosure enclosure) => _enclosures.Add(enclosure);
    
    public IEnumerable<Enclosure> GetAll() => _enclosures;
    
    public Enclosure GetById(Guid id) => _enclosures.FirstOrDefault(a => a.Id == id) ?? 
                                         throw new KeyNotFoundException("Enclosure not found");
    
    public void Remove(Enclosure enclosure) => _enclosures.Remove(enclosure);

    public void Update(Enclosure newEnclosureModel, Guid enclosureId)
    {
        var existing = _enclosures.FirstOrDefault(a => a.Id == enclosureId);
        if (existing == null) 
            throw new ArgumentException($"Enclosure with id: {enclosureId} does not exist!");
        
        newEnclosureModel.Id = existing.Id;
        _enclosures.Remove(existing);
        _enclosures.Add(newEnclosureModel);
    }
}