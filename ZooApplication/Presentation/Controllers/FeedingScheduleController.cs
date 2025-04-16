using System;
using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Presentation.Models;
using ZooApplication.Domain.Entities;

namespace ZooApplication.Presentation.Controllers
{
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
            return Ok(schedule);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateFeedingScheduleRequest request)
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

        // Метод для перепланирования времени кормления
        [HttpPut("{id}/reschedule")]
        public IActionResult Reschedule(Guid id, [FromBody] RescheduleFeedingRequest request)
        {
            try
            {
                var schedule = _feedingScheduleRepository.GetById(id);
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

        // Метод для обновления типа пищи в расписании
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] UpdateFeedingScheduleRequest request)
        {
            try
            {
                var schedule = _feedingScheduleRepository.GetById(id);

                schedule.ChangeFood(request.Food);
                _feedingScheduleRepository.Update(schedule);
                return Ok(schedule);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // Новый метод для удаления записи расписания кормления
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var schedule = _feedingScheduleRepository.GetById(id);

                _feedingScheduleRepository.Remove(schedule);
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
                var schedule = _feedingScheduleRepository.GetById(id);
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