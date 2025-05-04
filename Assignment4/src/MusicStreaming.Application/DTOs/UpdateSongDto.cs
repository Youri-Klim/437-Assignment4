using System;

namespace MusicStreaming.Application.DTOs
{
    public class UpdateSongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DurationInSeconds { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public int AlbumId { get; set; }
    }
}