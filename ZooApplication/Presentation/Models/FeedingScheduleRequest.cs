using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Presentation.Models;

/// <summary>
/// Feeding Schedule Request Model.
/// </summary>
public class FeedingScheduleRequest
{
    public Guid AnimalId { get; set; }

    public DateTime FeedingTime { get; set; }

    public string Food { get; set; }
}