namespace MusicStreaming.Application.DTOs
{
    public class UpdatePlaylistDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
    }
}