namespace AuthService.Application.DTOs
{
    public class ArticleDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string AuthorEmail { get; set; }
        public string Status { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<SectionDto> Sections { get; set; } = new();
    }
}
