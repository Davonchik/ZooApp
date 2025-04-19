using MediatR;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Events;

namespace ZooApplication.Application.BackgroundServices;

/// <summary>
/// Watcher for monitoring upcoming feedings.
/// </summary>
public class FeedingScheduleWatcher : BackgroundService
{
    private readonly IFeedingScheduleRepository _feedingScheduleRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public FeedingScheduleWatcher(IFeedingScheduleRepository feedingScheduleRepository,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _feedingScheduleRepository = feedingScheduleRepository;
        _domainEventDispatcher = domainEventDispatcher;
    }
    
    /// <summary>
    /// Runs a background loop that every 30 seconds checks all pending feeding schedules,
    /// and for each schedule whose time has arrived or passed, publishes "FeedingTimeEvent".
    /// </summary>
    /// <param name="stoppingToken">A cancellation token used to gracefully stop the background task
    /// (for example, when the application is shutting down).</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var times = _feedingScheduleRepository.GetAll()
                .Where(t => !t.IsCompleted && t.FeedingTime.Value <= now)
                .ToList();
            
            foreach (var time in times)
            {
                _domainEventDispatcher.Dispatch([new FeedingTimeEvent(time)]);
            }
            
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}