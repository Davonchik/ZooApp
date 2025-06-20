using ZooApplication.Domain.Common;

namespace ZooApplication.Presentation.Models;

/// <summary>
/// Create Animal Request Model.
/// </summary>
public class CreateAnimalRequest : AnimalRequest
{
    public Guid EnclosureId { get; set; }

    public AnimalTypeValue Species { get; set; }
}