namespace AuthService.Application.DTOs.Events
{
    public record UserRegisteredEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = string.Empty;
        public string FullName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
