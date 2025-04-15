using Microsoft.AspNetCore.Mvc;
using ZooApp.Application.Interfaces;
using ZooApp.Application.Services;

namespace ZooApp.Presentation.Controllers;

public class RescheduleFeedingRequest
{
    public DateTime NewFeedingTime { get; set; }
}

// DTO для создания новой записи расписания кормления
public class CreateFeedingScheduleRequest
{
    public Guid AnimalId { get; set; }
    // Используем имя FeedingTime, чтобы отличать от резкейл-запроса
    public DateTime FeedingTime { get; set; }
    public string Food { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class FeedingScheduleController : ControllerBase
{
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;
    private readonly FeedingOrganizationService _feedingOrganizationService;

    public FeedingScheduleController(
        IFeedingScheduleRepository feedingScheduleRepository,
        FeedingOrganizationService feedingOrganizationService)
    {
        _feedingScheduleRepository = feedingScheduleRepository;
        _feedingOrganizationService = feedingOrganizationService;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_feedingScheduleRepository.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var schedule = _feedingScheduleRepository.GetById(id);
        if (schedule == null)
        {
            return NotFound();
        }

        return Ok(schedule);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateFeedingScheduleRequest request)
    {
        try
        {
            // Используем FeedingTime из запроса
            var schedule = _feedingOrganizationService.ScheduleFeeding(request.AnimalId,
                request.FeedingTime, request.Food);
            return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/reschedule")]
    public IActionResult Reschedule(Guid id, [FromBody] RescheduleFeedingRequest request)
    {
        try
        {
            var schedule = _feedingScheduleRepository.GetById(id);
            if (schedule == null)
            {
                return NotFound();
            }

            schedule.Reschedule(request.NewFeedingTime);
            return Ok(schedule);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{id}/complete")]
    public IActionResult Complete(Guid id)
    {
        try
        {
            var schedule = _feedingScheduleRepository.GetById(id);
            if (schedule == null)
            {
                return NotFound();
            }

            schedule.MarkAsCompleted();
            return Ok(schedule);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}