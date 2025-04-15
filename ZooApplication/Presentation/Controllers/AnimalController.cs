using Microsoft.AspNetCore.Mvc;
using ZooApp.Application.Interfaces;
using ZooApp.Application.Services;
using ZooApp.Domain.Entities;
using ZooApp.Domain.ValueObjects;

namespace ZooApp.Presentation.Controllers;

public class CreateAnimalRequest
{
    public string Name { get; set; }
    public string Species { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string FavoriteFood { get; set; }
    public Guid EnclosureId { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly IAnimalRepository _animalRepository;
    private readonly AnimalTransferService _transferService;

    public AnimalController(IAnimalRepository animalRepository, AnimalTransferService transferService)
    {
        _animalRepository = animalRepository;
        _transferService = transferService;
    }
    
    [HttpGet]
    public IActionResult GetAll() => Ok(_animalRepository.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var animal = _animalRepository.GetById(id);
        if (animal == null)
            return NotFound();
        return Ok(animal);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateAnimalRequest request)
    {
        try
        {
            var animal = new Animal(
                new AnimalName(request.Name),
                request.Species,
                request.BirthDate,
                request.Gender,
                request.FavoriteFood
            );

            _animalRepository.Add(animal);
            
            if (request.EnclosureId != Guid.Empty)
                _transferService.TransferAnimal(animal.Id, request.EnclosureId);
            
            return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var animal = _animalRepository.GetById(id);
        if (animal == null)
        {
            return NotFound();
        }
        _animalRepository.Remove(animal);
        return NoContent();
    }

    [HttpPost("{id}/move")]
    public IActionResult Move(Guid id, [FromQuery] Guid targetEnclosureId)
    {
        try
        {
            _transferService.TransferAnimal(id, targetEnclosureId);
            var animal = _animalRepository.GetById(id);
            return Ok(animal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
