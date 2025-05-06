using System.Collections.Generic;

namespace MusicStreaming.Application.DTOs
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
        public required string Genre { get; set; }
        public int ArtistId { get; set; }
        public required string ArtistName { get; set; }
        public IList<SongDto> Songs { get; set; } = new List<SongDto>();

    }
}