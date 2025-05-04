namespace MusicStreaming.Application.DTOs
{
    public class CreateAlbumDto
    {
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public int ArtistId { get; set; }
    }
}