using ZooApplication.Domain.Common;

namespace ZooApplication.Presentation.Models;

public class CreateAnimalRequest : AnimalRequest
{
    public Guid EnclosureId { get; set; }
    
    public AnimalTypeValue Species { get; set; }
}