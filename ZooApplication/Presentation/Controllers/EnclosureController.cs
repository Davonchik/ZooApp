using Microsoft.AspNetCore.Mvc;
using ZooApp.Application.Interfaces;
using ZooApp.Application.Services;
using ZooApp.Domain.Entities;

namespace ZooApp.Presentation.Controllers;

// DTO для создания нового вольера.
public class CreateEnclosureRequest
{
    // Тип вольера, например: "хищники", "травоядные", "птицы", "аквариум"
    public string EnclosureType { get; set; }
    
    // Размер вольера (например, в квадратных метрах)
    public double Size { get; set; }
    
    // Максимальное количество животных, которое может содержаться в вольере
    public int MaximumCapacity { get; set; }
}

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
        if (enclosure == null)
            return NotFound();
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
        if (enclosure == null)
            return NotFound();
        _enclosureRepository.Remove(enclosure);
        return NoContent();
    }

    // POST: api/enclosure/{id}/clean
    // Выполняет уборку в вольере, обновляя дату последней уборки.
    [HttpPost("{id}/clean")]
    public IActionResult Clean(Guid id)
    {
        try
        {
            var enclosure = _enclosureRepository.GetById(id);
            if (enclosure == null)
                return NotFound();

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