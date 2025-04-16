namespace ZooApplication.Presentation.Models;

public class CreateFeedingScheduleRequest
{
    public Guid AnimalId { get; set; }
    
    public DateTime FeedingTime { get; set; }
    
    public string Food { get; set; }
}