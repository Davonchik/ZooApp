using MediatR;
using ZooApplication.Application.Interfaces;
using ZooApplication.Domain.Events;

namespace ZooApplication.Application.BackgroundServices;

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