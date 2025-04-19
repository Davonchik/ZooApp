using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Common;
using ZooApplication.Domain.Entities;
using ZooApplication.Domain.ValueObjects;
using ZooApplication.Presentation.Models;

namespace ZooApplication.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedingScheduleController : ControllerBase
    {
        private readonly IFeedingOrganizationService _feedingOrganizationService;

        public FeedingScheduleController(IFeedingOrganizationService feedingOrganizationService)
        {
            _feedingOrganizationService = feedingOrganizationService;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_feedingOrganizationService.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var schedule = _feedingOrganizationService.GetById(id);
            return Ok(schedule);
        }

        [HttpPost]
        public IActionResult Create([FromBody] FeedingScheduleRequest request)
        {
            try
            {
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
                var schedule = _feedingOrganizationService.GetById(id);
                if (schedule == null)
                    return NotFound();

                schedule.Reschedule(request.NewFeedingTime);
                return Ok(schedule);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] FeedingScheduleRequest request)
        {
            try
            {
                var newModel = new FeedingSchedule(
                    request.AnimalId,
                    new FeedingTime(request.FeedingTime),
                    new Food(new Name(request.Food), new AnimalType(AnimalTypeValue.Default))
                );
                
                _feedingOrganizationService.UpdateSchedule(newModel, id);
                
                var updated = _feedingOrganizationService.GetById(id);
                return Ok(updated);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _feedingOrganizationService.DeleteSchedule(id);
                return NoContent();
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
                var schedule = _feedingOrganizationService.GetById(id);
                if (schedule == null)
                    return NotFound();

                schedule.MarkAsCompleted();
                return Ok(schedule);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}