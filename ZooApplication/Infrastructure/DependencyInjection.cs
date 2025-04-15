using Microsoft.Extensions.DependencyInjection;
using ZooApp.Application.Interfaces;
using ZooApp.Application.Services;
using ZooApp.Infrastructure.Repositories;

namespace ZooApp.Infrastructure;

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