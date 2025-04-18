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
        services.AddTransient<IAnimalService, AnimalService>();
        services.AddScoped<IAnimalTransferService, AnimalTransferService>();
        services.AddScoped<IEnclosureService, EnclosureService>();
        services.AddScoped<IFeedingOrganizationService, FeedingOrganizationService>();
        services.AddScoped<IZooStatisticsService, ZooStatisticsService>();
        return services;
    }
}