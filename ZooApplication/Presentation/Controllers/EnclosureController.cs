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
    private readonly IEnclosureService _enclosureService;

    public EnclosureController(IEnclosureService enclosureService)
    {
        _enclosureService = enclosureService;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_enclosureService.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id) => Ok(_enclosureService.GetById(id));

    [HttpPost]
    public IActionResult Create([FromBody] EnclosureRequest request)
    {
        try
        {
            var enclosure = new Enclosure(request.Name, new Capacity(request.MaximumCapacity), request.EnclosureType);
            var created = _enclosureService.CreateEnclosure(enclosure);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
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
            
            _enclosureService.UpdateEnclosure(id, enclosure);

            return Ok(enclosure);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("{id}/clean")]
    public IActionResult Clean(Guid id)
    {
        try
        {
            var cleaned = _enclosureService.CleanEnclosure(id);
            return Ok(cleaned);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}