using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Application.Dto;

/// <summary>
/// Zoo Statistics DTO.
/// </summary>
public class ZooStatisticsDto
{
    public int TotalAnimals { get; set; }

    public int TotalEnclosures { get; set; }

    public int EmptyEnclosures { get; set; }

    public Dictionary<string, int> AnimalsByEnclosure { get; set; }

    public int UpcomingFeedings { get; set; }
}