using System.Collections.Generic;

namespace MusicStreaming.Application.DTOs
{
    public class ArtistDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Genre { get; set; }
        public IList<AlbumDto> Albums { get; set; } = new List<AlbumDto>();
    }
}