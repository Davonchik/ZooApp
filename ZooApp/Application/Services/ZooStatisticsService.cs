using ZooApp.Application.Interfaces;

namespace ZooApp.Application.Services;

public class ZooStatisticsService
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
            e => animals.Count(a => a.EnclosureId == e.Id)
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

public class ZooStatisticsDto
{
    public int TotalAnimals { get; set; }
    public int TotalEnclosures { get; set; }
    public Dictionary<string, int> AnimalsByEnclosure { get; set; }
    public int UpcomingFeedings { get; set; }
}