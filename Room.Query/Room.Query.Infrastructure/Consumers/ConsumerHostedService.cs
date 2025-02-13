using CQRS.Core.Topics;
using CQRS.CORE.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Room.Query.Infrastructure.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _topic;
    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider, IOptions<KafkaTopic> config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _topic = config.Value.Topic;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Consumer start consumming");
        using (
            IServiceScope scope = _serviceProvider.CreateScope()) { 
            var Consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
            Task.Run(() => Consumer.Consume(_topic));
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Event Consumer service stoped");
        return Task.CompletedTask;
    }
}
