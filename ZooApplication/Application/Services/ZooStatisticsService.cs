using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Dto;

namespace ZooApplication.Application.Services;

/// <summary>
/// Zoo Statistics Service.
/// </summary>
public class ZooStatisticsService : IZooStatisticsService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;

    public ZooStatisticsService(IAnimalRepository animalRepository, IEnclosureRepository enclosureRepository,
        IFeedingScheduleRepository feedingScheduleRepository)
    {
        _animalRepository = animalRepository;
        _enclosureRepository = enclosureRepository;
        _feedingScheduleRepository = feedingScheduleRepository;
    }

    /// <summary>
    /// Method for getting Zoo Statistics.
    /// </summary>
    /// <returns>Zoo Statistics DTO.</returns>
    public ZooStatisticsDto GetZooStatistics()
    {
        var animals = _animalRepository.GetAll();

        var enclosures = _enclosureRepository.GetAll();

        var emptyEnclosures = enclosures.Count(animal => animal.CurrentAnimalCount == 0);

        var feedingSchedules = _feedingScheduleRepository.GetAll()
            .Where(f => f.FeedingTime.Value >= DateTime.UtcNow).ToList();

        var animalsByEnclosures = enclosures.ToDictionary(
            e => e.Name.Value,
            e => e.CurrentAnimalCount
        );

        return new ZooStatisticsDto
        {
            TotalAnimals = animals.Count(),
            TotalEnclosures = enclosures.Count(),
            EmptyEnclosures = emptyEnclosures,
            AnimalsByEnclosure = animalsByEnclosures,
            UpcomingFeedings = feedingSchedules.Count
        };
    }
}