using System;

namespace MusicStreaming.Application.DTOs
{
    public class SongDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Duration { get; set; }
        public string DurationFormatted => TimeSpan.FromSeconds(Duration).ToString(@"mm\:ss");
        public DateTime ReleaseDate { get; set; }
        public required string Genre { get; set; }
        public int AlbumId { get; set; }
        public required string AlbumTitle { get; set; }
        public required string ArtistName { get; set; }
    }
}