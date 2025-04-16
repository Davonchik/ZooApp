using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Presentation.Models;

namespace ZooApplication.Presentation.Controllers;

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
        
        _animalRepository.Remove(animal);
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UpdateAnimalRequest request)
    {
        try
        {
            var animal = _animalRepository.GetById(id);
            
            animal.ChangeName(new AnimalName(request.Name));
            _animalRepository.Update(animal);

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
            var animal = _animalRepository.GetById(id);
            return Ok(animal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
