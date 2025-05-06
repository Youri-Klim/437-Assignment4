namespace MusicStreaming.Application.DTOs
{
    public class CreateArtistDto
    {
        public required string Name { get; set; }
        public required string Genre { get; set; }
    }
}