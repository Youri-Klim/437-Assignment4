namespace MusicStreaming.Application.DTOs
{
    public class CreatePlaylistDto
    {
        public required string Title { get; set; }
        public required string UserId { get; set; }
    }
}