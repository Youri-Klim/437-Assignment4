using System;

namespace MusicStreaming.Application.DTOs
{
    public class CreateSongDto
    {
        public required string Title { get; set; }
        public int DurationInSeconds { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Genre { get; set; }
        public int AlbumId { get; set; }
    }
}