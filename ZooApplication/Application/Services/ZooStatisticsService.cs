using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Dto;

namespace ZooApplication.Application.Services;

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

    public ZooStatisticsDto GetZooStatistics()
    {
        var animals = _animalRepository.GetAll();
        var enclosures = _enclosureRepository.GetAll();
        var feedingSchedules = _feedingScheduleRepository.GetAll()
            .Where(f => f.FeedingTime.Value >= DateTime.UtcNow).ToList();
        
        var animalsByEnclosures = enclosures.ToDictionary(
            e => e.Name,
            e => e.CurrentAnimalCount
        );

        return new ZooStatisticsDto
        {
            TotalAnimals = animals.Count(),
            TotalEnclosures = enclosures.Count(),
            AnimalsByEnclosure = animalsByEnclosures,
            UpcomingFeedings = feedingSchedules.Count
        };
    }
}