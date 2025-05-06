namespace MusicStreaming.Application.DTOs
{
    public class UserProfileDto
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public int PlaylistCount { get; set; }
    }
}