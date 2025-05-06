namespace MusicStreaming.Application.DTOs
{
    public class UpdateUserDto
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
}