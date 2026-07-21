using AuthService.Domain.Common;
using AuthService.Domain.Enum;
using AuthService.Domain.Events;
using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Entities
{
    public class Article: BaseEntity<Guid>, AggregateRoot
    {
        public string Title { get; private set; }
        public string AuthorEmail { get; private set; }
        public ArticleStatus Status { get; set; }
        public DateTime? PublishedAt { get; private set; }


        // Navigation Property
        private readonly List<Section> _sections = new();
        public IReadOnlyCollection<Section> Sections => _sections.AsReadOnly();

        private Article() { }

        public static Article Create(string title, string authorEmail)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title cannot be empty.");

            var article = new Article
            {
                Id = Guid.NewGuid(),
                Title = title,
                AuthorEmail = authorEmail,
                Status = ArticleStatus.Draf,
                PublishedAt = null
            };

            return article;
        }

        public void Publish()
        {
            if (Status == ArticleStatus.Published)
                return;

            if (Status == ArticleStatus.Archived)
                throw new InvalidOperationException("Cannot publish an archived article.");

            // Thay đổi State
            Status = ArticleStatus.Published;
            PublishedAt = DateTime.UtcNow;

            // Sinh ra Domain Event
            AddDomainEvent(new ArticlePublishedDomainEvent
            {
                Id = this.Id,
                Title = this.Title,
                AuthorEmail = this.AuthorEmail,
                ArticleStatus = this.Status,
                PublishedAt = this.PublishedAt
            });
        }

        public void AddSection(string title, string content)
        {
            if (Status == ArticleStatus.Archived)
                throw new InvalidOperationException("Cannot add sections to an archived article.");

            var section = new Section(title, content);
            _sections.Add(section);
        }
    }
}
