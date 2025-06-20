using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Infrastructure.Repositories;

/// <summary>
/// In Memory Enclosure Repository.
/// </summary>
public class InMemoryEnclosureRepository : IEnclosureRepository
{
    private readonly List<Enclosure> _enclosures = new List<Enclosure>();

    /// <summary>
    /// Method for adding enclosure to repository.
    /// </summary>
    /// <param name="enclosure">Enclosure.</param>
    public void Add(Enclosure enclosure) => _enclosures.Add(enclosure);

    /// <summary>
    /// Method for getting all enclosures.
    /// </summary>
    /// <returns>IEnumerable of enclosures.</returns>
    public IEnumerable<Enclosure> GetAll() => _enclosures;

    /// <summary>
    /// Method for getting Enclosure by ID.
    /// </summary>
    /// <param name="id">Enclosure's ID.</param>
    /// <returns>Enclosure.</returns>
    /// <exception cref="KeyNotFoundException">Exception.</exception>
    public Enclosure GetById(Guid id) => _enclosures.FirstOrDefault(a => a.Id == id) ??
                                         throw new KeyNotFoundException("Enclosure not found");

    /// <summary>
    /// Method for removing enclosure.
    /// </summary>
    /// <param name="enclosure">Enclosure.</param>
    public void Remove(Enclosure enclosure) => _enclosures.Remove(enclosure);

    /// <summary>
    /// Method for updating info about enclosure.
    /// </summary>
    /// <param name="newEnclosureModel">New Enclosure Model.</param>
    /// <param name="enclosureId">Enclosure's ID.</param>
    /// <exception cref="ArgumentException">Exception.</exception>
    public void Update(Enclosure newEnclosureModel, Guid enclosureId)
    {
        var existing = _enclosures.FirstOrDefault(a => a.Id == enclosureId);

        if (existing == null)
            throw new ArgumentException($"Enclosure with id: {enclosureId} does not exist!");

        if (existing.CurrentAnimalCount > newEnclosureModel.MaximumCapacity.Value)
            throw new ArgumentException("Cannot make it so small - there are to many animals!");

        if (newEnclosureModel.EnclosureType.Value != existing.EnclosureType.Value &&
            existing.CurrentAnimalCount > 0)
            throw new ArgumentException("There are already animals with another type!");


        newEnclosureModel.Id = existing.Id;
        _enclosures.Remove(existing);
        _enclosures.Add(newEnclosureModel);
    }
}