namespace MusicStreaming.Application.DTOs
{
    public class UpdateArtistDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Genre { get; set; }
    }
}