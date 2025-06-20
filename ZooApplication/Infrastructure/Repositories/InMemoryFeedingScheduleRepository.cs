using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Infrastructure.Repositories;

/// <summary>
/// In Memory Feeding Schedule Repository.
/// </summary>
public class InMemoryFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly List<FeedingSchedule> _feedingSchedules = new List<FeedingSchedule>();

    /// <summary>
    /// Method for Feeding Schedule addition.
    /// </summary>
    /// <param name="feedingSchedule">Feeding Schedule.</param>
    public void Add(FeedingSchedule feedingSchedule) => _feedingSchedules.Add(feedingSchedule);

    /// <summary>
    /// Method for getting all Feeding Schedules.
    /// </summary>
    /// <returns>IEnumerable of Feeding Schedules.</returns>
    public IEnumerable<FeedingSchedule> GetAll() => _feedingSchedules;

    /// <summary>
    /// Method for getting Feeding Schedule by ID.
    /// </summary>
    /// <param name="id">Feeding Schedule's ID.</param>
    /// <returns>Feeding Schedule.</returns>
    /// <exception cref="KeyNotFoundException">Exception.</exception>
    public FeedingSchedule GetById(Guid id) => _feedingSchedules.FirstOrDefault(a => a.Id == id) ??
                                               throw new KeyNotFoundException("Feeding schedule not found");

    /// <summary>
    /// Method for getting upcoming feedings events.
    /// </summary>
    /// <param name="from">Search Start Time.</param>
    /// <returns>IEnumerable of Feeding Schedules.</returns>
    public IEnumerable<FeedingSchedule> GetUpcoming(DateTime from) => _feedingSchedules
        .Where(f => f.FeedingTime.Value >= from);

    /// <summary>
    /// Method for Feeding Schedules removing.
    /// </summary>
    /// <param name="feedingSchedule">Feeding Schedule.</param>
    public void Remove(FeedingSchedule feedingSchedule) => _feedingSchedules.Remove(feedingSchedule);

    /// <summary>
    /// Method for Feeding Schedule Info updating.
    /// </summary>
    /// <param name="newFeedingScheduleModel">New Feeding Schedule.</param>
    /// <param name="feedingScheduleId">Feeding Schedule's ID.</param>
    /// <exception cref="KeyNotFoundException">Exception.</exception>
    public void Update(FeedingSchedule newFeedingScheduleModel, Guid feedingScheduleId)
    {
        var existing = _feedingSchedules.FirstOrDefault(a => a.Id == feedingScheduleId);

        if (existing == null)
            throw new KeyNotFoundException("Feeding schedule not found");

        newFeedingScheduleModel.Id = existing.Id;
        _feedingSchedules.Remove(existing);
        _feedingSchedules.Add(newFeedingScheduleModel);
    }
}