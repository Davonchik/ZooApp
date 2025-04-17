using ZooApplication.Domain.Common;
using ZooApplication.Domain.ValueObjects;

namespace ZooApplication.Presentation.Models;

public class EnclosureRequest
{
    public Name Name { get; set; }
    
    public AnimalType EnclosureType { get; set; }
    
    public int MaximumCapacity { get; set; }
}