using AuthService.Application.Common.Interfaces;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Services
{
    public class IntegrationEventService : IIntegrationEventService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<IntegrationEventService> _logger;

        public IntegrationEventService(IPublishEndpoint publishEndpoint, ILogger<IntegrationEventService> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : class
        {
            _logger.LogInformation("Publishing integration event {EventType}", typeof(T).Name);

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}
