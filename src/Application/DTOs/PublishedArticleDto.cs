namespace AuthService.Application.DTOs
{
    public class PublishedArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime PublishedAt { get; set; }
        public int SectionCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
