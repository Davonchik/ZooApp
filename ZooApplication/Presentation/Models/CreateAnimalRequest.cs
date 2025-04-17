using ZooApplication.Domain.Common;

namespace ZooApplication.Presentation.Models;

public class CreateAnimalRequest : AnimalRequest
{
    public AnimalTypeValue Species { get; set; }
}