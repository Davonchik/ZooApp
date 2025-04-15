using ZooApp.Application.Interfaces;
using ZooApp.Domain.Entities;

namespace ZooApp.Infrastructure.Repositories;

public class InMemoryEnclosureRepository : IEnclosureRepository
{
    private readonly List<Enclosure> _enclosures = new List<Enclosure>();
    public void Add(Enclosure enclosure) => _enclosures.Add(enclosure);
    public IEnumerable<Enclosure> GetAll() => _enclosures;
    public Enclosure GetById(Guid id) => _enclosures.FirstOrDefault(a => a.Id == id);
    public void Remove(Enclosure enclosure) => _enclosures.Remove(enclosure);

    public void Update(Enclosure enclosure)
    {
        // var existing = _enclosures.FirstOrDefault(a => a.Id == enclosure.Id);
        // if (existing == null)
        // {
        //     throw new ArgumentException($"Enclosure with id: {enclosure.Id} does not exist!");
        // }
    }
}