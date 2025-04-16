namespace ZooApplication.Application.Interfaces;

public interface IAnimalTransferService
{
    void TransferAnimal(Guid animalId, Guid targetEnclosureId);
}