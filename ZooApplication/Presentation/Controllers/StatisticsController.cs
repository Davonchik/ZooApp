using Microsoft.AspNetCore.Mvc;
using ZooApplication.Application.Services;

namespace ZooApplication.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly ZooStatisticsService _statisticsService;

    public StatisticsController(ZooStatisticsService statisticsService)
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