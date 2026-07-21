using AuthService.Application.Common.Interfaces;
using AuthService.Domain.Events;
using MediatR;
using Shared.Contracts.Events;

namespace AuthService.Application.Features.Article.EventHandlers
{
    public class ArticlePublishedDomainEventHandler : INotificationHandler<ArticlePublishedDomainEvent>
    {
        private readonly IIntegrationEventService _integrationEvent;

        public ArticlePublishedDomainEventHandler(IIntegrationEventService integrationEvent)
        {
            _integrationEvent = integrationEvent;
        }

        public async Task Handle(ArticlePublishedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var integrationEvent = new ArticlePublishedIntegrationEvent
            {
                ArticleId = domainEvent.Id,
                Title = domainEvent.Title,
                AuthorEmail = domainEvent.AuthorEmail,
                PublishedAt = domainEvent.PublishedAt ?? DateTime.UtcNow
            };

            await _integrationEvent.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
