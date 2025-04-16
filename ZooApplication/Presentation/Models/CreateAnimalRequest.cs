using ZooApplication.Domain.Entities;

namespace ZooApplication.Presentation.Models;

public class CreateAnimalRequest
{
    public string Name { get; set; }
    public string Species { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string FavoriteFood { get; set; }
    public Guid EnclosureId { get; set; }
}