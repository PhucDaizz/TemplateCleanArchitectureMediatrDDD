namespace AuthService.Application.DTOs
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AuthorEmail { get; set; } = string.Empty;
        public string Status { get; set; }                
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }         
        public DateTime? UpdatedAt { get; set; }        
    }
}
