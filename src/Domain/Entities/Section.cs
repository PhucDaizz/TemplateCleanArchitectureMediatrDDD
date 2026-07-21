using AuthService.Domain.Common;

namespace AuthService.Domain.Entities
{
    public class Section: BaseEntity<Guid>
    {
        public Guid ArticleId { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public int Order { get; private set; }

        private Section() { }

        public Section(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
