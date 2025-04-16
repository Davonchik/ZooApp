using Microsoft.Extensions.DependencyInjection;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Infrastructure.Repositories;

namespace ZooApplication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
        services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
        services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AnimalTransferService>();
        services.AddScoped<FeedingOrganizationService>();
        services.AddScoped<ZooStatisticsService>();
        return services;
    }
}