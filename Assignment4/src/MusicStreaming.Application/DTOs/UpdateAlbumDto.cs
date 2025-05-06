namespace MusicStreaming.Application.DTOs
{
    public class UpdateAlbumDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
        public required string Genre { get; set; }
        public int ArtistId { get; set; }
    }
}