using AuthService.Domain.Common;
using AuthService.Domain.Enum;

namespace AuthService.Domain.Events
{
    public record ArticlePublishedDomainEvent : DomainEvent
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorEmail { get; set; }
        public ArticleStatus ArticleStatus { get; set; }
        public DateTime? PublishedAt { get; set; }
    }       
}
