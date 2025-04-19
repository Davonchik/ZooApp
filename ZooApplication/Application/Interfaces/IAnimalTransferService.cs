namespace ZooApplication.Application.Interfaces;

/// <summary>
/// Interface of Transfer Service.
/// </summary>
public interface IAnimalTransferService
{
    void TransferAnimal(Guid animalId, Guid targetEnclosureId);
}