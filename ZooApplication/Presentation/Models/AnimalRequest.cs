using ZooApplication.Domain.Common;

namespace ZooApplication.Presentation.Models;

/// <summary>
/// Animal Request Model.
/// </summary>
public class AnimalRequest
{
    public string Name { get; set; }

    public DateTime BirthDate { get; set; }

    public GenderValue GenderValue { get; set; }

    public string FavoriteFood { get; set; }

    public HealthStatusValue HealthStatus { get; set; }
}