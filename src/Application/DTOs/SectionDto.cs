namespace AuthService.Application.DTOs
{
    public class SectionDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }
    }
}
