using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Interfaces;

namespace ZooApplication.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IZooStatisticsService _statisticsService;

    public StatisticsController(IZooStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public IActionResult GetStatistics()
    {
        var stats = _statisticsService.GetZooStatistics();
        return Ok(stats);
    }
}