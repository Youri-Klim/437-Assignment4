namespace MusicStreaming.Application.DTOs
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
}