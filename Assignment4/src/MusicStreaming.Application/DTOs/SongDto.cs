using System;

namespace MusicStreaming.Application.DTOs
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DurationInSeconds { get; set; }
        public string DurationFormatted => TimeSpan.FromSeconds(DurationInSeconds).ToString(@"mm\:ss");
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public string ArtistName { get; set; }
    }
}