using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Application.Dto;

public class ZooStatisticsDto
{
    public int TotalAnimals { get; set; }
    
    public int TotalEnclosures { get; set; }
    
    public int EmptyEnclosures { get; set; }
    
    public Dictionary<string, int> AnimalsByEnclosure { get; set; }
    
    public int UpcomingFeedings { get; set; }
}