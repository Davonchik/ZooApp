using Microsoft.Extensions.DependencyInjection;
using ZooApplication.Application.BackgroundServices;
using ZooApplication.Application.Dispatcher;
using ZooApplication.Application.Interfaces;
using ZooApplication.Application.Services;
using ZooApplication.Infrastructure.Repositories;

namespace ZooApplication.Infrastructure;

/// <summary>
/// Extension methods to register Infrastructure and Application services into DI.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers all Infrastructure‑layer dependencies.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAnimalRepository, InMemoryAnimalRepository>();
        services.AddSingleton<IEnclosureRepository, InMemoryEnclosureRepository>();
        services.AddSingleton<IFeedingScheduleRepository, InMemoryFeedingScheduleRepository>();
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddHostedService<FeedingScheduleWatcher>();
        return services;
    }

    /// <summary>
    /// Registers all Application‑layer services.
    /// </summary>
    /// <param name="services">Services.</param>
    /// <returns>IServiceCollection.</returns>
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