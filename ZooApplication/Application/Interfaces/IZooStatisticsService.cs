using ZooApplication.Application.Dto;

namespace ZooApplication.Application.Interfaces;

/// <summary>
/// Interface of Zoo Statistics Service.
/// </summary>
public interface IZooStatisticsService
{
    ZooStatisticsDto GetZooStatistics();
}