using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Presentation.Models;

namespace ZooApplication.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private readonly IAnimalTransferService _transferService;
    private readonly IAnimalService _animalService;

    public AnimalController(IAnimalTransferService transferService, 
        IAnimalService animalService)
    {
        _transferService = transferService;
        _animalService = animalService;
    }
    
    [HttpGet]
    public IActionResult GetAll() => Ok(_animalService.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var animal = _animalService.GetById(id);
        
        return Ok(animal);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateAnimalRequest request)
    {
        var animal = new Animal(
            new Name(request.Name),
            new AnimalType(request.Species),
            request.BirthDate,
            new Gender(request.GenderValue),
            new Food(new Name(request.FavoriteFood), new AnimalType(request.Species)),
            new HealthStatus(request.HealthStatus)
        );
        
        try
        {
            _animalService.CreateAnimal(animal, request.EnclosureId);
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
        _animalService.DeleteAnimal(id);
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] AnimalRequest request)
    {
        try
        {
            var animal = new Animal(
                new Name(request.Name),
                new AnimalType(AnimalTypeValue.Default),
                request.BirthDate,
                new Gender(request.GenderValue),
                new Food(new Name(request.FavoriteFood), new AnimalType(AnimalTypeValue.Default)),
                new HealthStatus(request.HealthStatus)
            );
            
            _animalService.UpdateAnimal(animal, id);

            return Ok(animal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/move")]
    public IActionResult Move(Guid id, [FromQuery] Guid targetEnclosureId)
    {
        try
        {
            _transferService.TransferAnimal(id, targetEnclosureId);
            var animal = _animalService.GetById(id);
            return Ok(animal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/heal")]
    public IActionResult Heal(Guid id)
    {
        try
        {
            _animalService.Heal(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/feed")]
    public IActionResult Feed(Guid id)
    {
        try
        {
            _animalService.Feed(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
