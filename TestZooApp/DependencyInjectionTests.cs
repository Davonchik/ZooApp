using Microsoft.Extensions.DependencyInjection;
using ZooApplication.Application.BackgroundServices;
using ZooApplication.Application.Interfaces;
using ZooApplication.Infrastructure;

namespace TestZooApp;

public class DependencyInjectionTests
{
    private ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection()
            .AddInfrastructure()
            .AddApplication();
        return services.BuildServiceProvider();
    }

    [Fact]
    public void Repositories_AreRegisteredAsSingletons()
    {
        // Arrange
        var provider = BuildProvider();

        // Act
        var repo1 = provider.GetRequiredService<IAnimalRepository>();
        var repo2 = provider.GetRequiredService<IAnimalRepository>();

        // Assert
        Assert.Same(repo1, repo2);
    }

    [Fact]
    public void AnimalService_IsTransient()
    {
        // Arrange
        var provider = BuildProvider();

        // Act
        var s1 = provider.GetRequiredService<IAnimalService>();
        var s2 = provider.GetRequiredService<IAnimalService>();

        // Assert
        Assert.NotSame(s1, s2);
    }

    [Fact]
    public void ScopedServices_ReturnSameInstanceWithinScope_DifferentAcrossScopes()
    {
        // Arrange
        var provider = BuildProvider();

        // Act
        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var scoped1a = scope1.ServiceProvider.GetRequiredService<IEnclosureService>();
        var scoped1b = scope1.ServiceProvider.GetRequiredService<IEnclosureService>();

        var scoped2 = scope2.ServiceProvider.GetRequiredService<IEnclosureService>();

        // Assert
        Assert.Same(scoped1a, scoped1b);
        Assert.NotSame(scoped1a, scoped2);
    }

    [Fact]
    public void HostedService_AndDispatcher_AreRegistered()
    {
        // Arrange
        var provider = BuildProvider();

        // Act
        var dispatcher = provider.GetRequiredService<IDomainEventDispatcher>();
        var hosted = provider.GetServices<Microsoft.Extensions.Hosting.IHostedService>();

        // Assert
        Assert.NotNull(dispatcher);
        Assert.Contains(hosted, h => h is FeedingScheduleWatcher);
    }
}