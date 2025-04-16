using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Domain.Entities;
using ZooApplication.Presentation.Models;

namespace ZooApplication.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnclosureController : ControllerBase
{
    private readonly IEnclosureRepository _enclosureRepository;

    public EnclosureController(IEnclosureRepository enclosureRepository)
    {
        _enclosureRepository = enclosureRepository;
    }

    // GET: api/enclosure
    [HttpGet]
    public IActionResult GetAll() => Ok(_enclosureRepository.GetAll());

    // GET: api/enclosure/{id}
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var enclosure = _enclosureRepository.GetById(id);
        
        return Ok(enclosure);
    }

    // POST: api/enclosure
    [HttpPost]
    public IActionResult Create([FromBody] CreateEnclosureRequest request)
    {
        try
        {
            // Создаём вольер согласно переданным данным.
            var enclosure = new Enclosure(request.EnclosureType, request.Size, request.MaximumCapacity);
            _enclosureRepository.Add(enclosure);
            return CreatedAtAction(nameof(GetById), new { id = enclosure.Id }, enclosure);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/enclosure/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var enclosure = _enclosureRepository.GetById(id);
        
        _enclosureRepository.Remove(enclosure);
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] UpdateEnclosureRequest request)
    {
        try
        {
            var enclosure = _enclosureRepository.GetById(id);
            
            enclosure.ChangeName(request.Name);
            _enclosureRepository.Update(enclosure);

            return Ok(enclosure);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // POST: api/enclosure/{id}/clean
    // Выполняет уборку в вольере, обновляя дату последней уборки.
    [HttpPost("{id}/clean")]
    public IActionResult Clean(Guid id)
    {
        try
        {
            var enclosure = _enclosureRepository.GetById(id);

            enclosure.Clean(); // Метод Clean обновляет свойство LastCleaned.
            _enclosureRepository.Update(enclosure);
            return Ok(enclosure);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}