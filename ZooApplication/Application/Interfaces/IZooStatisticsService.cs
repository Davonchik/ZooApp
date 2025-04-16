using ZooApplication.Application.Dto;

namespace ZooApplication.Application.Interfaces;

public interface IZooStatisticsService
{
    ZooStatisticsDto GetZooStatistics();
}