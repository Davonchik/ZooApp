using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Presentation.Models;

namespace ZooApplication.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnclosureController : ControllerBase
{
    private readonly IEnclosureRepository _enclosureRepository;
    private readonly IEnclosureService _enclosureService;

    public EnclosureController(IEnclosureRepository enclosureRepository, IEnclosureService enclosureService)
    {
        _enclosureRepository = enclosureRepository;
        _enclosureService = enclosureService;
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
    public IActionResult Create([FromBody] EnclosureRequest request)
    {
        try
        {
            var enclosure = new Enclosure(request.Name, new Capacity(request.MaximumCapacity), request.EnclosureType);
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
        _enclosureService.DeleteEnclosure(id);
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] EnclosureRequest request)
    {
        try
        {
            var enclosure = new Enclosure(request.Name, new Capacity(request.MaximumCapacity), request.EnclosureType);
            
            _enclosureRepository.Update(enclosure, id);

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

            enclosure.Clean();
            return Ok(enclosure);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}