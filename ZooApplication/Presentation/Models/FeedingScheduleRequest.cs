using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Presentation.Models;

public class FeedingScheduleRequest
{
    public Guid AnimalId { get; set; }
    
    public DateTime FeedingTime { get; set; }
    
    public Food Food { get; set; }
}