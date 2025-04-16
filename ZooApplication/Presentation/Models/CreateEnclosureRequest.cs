namespace ZooApplication.Presentation.Models;

public class CreateEnclosureRequest
{
    public string EnclosureType { get; set; }
    
    public double Size { get; set; }
    
    public int MaximumCapacity { get; set; }
}