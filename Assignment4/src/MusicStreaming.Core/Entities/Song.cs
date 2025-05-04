namespace MusicStreaming.Core.Entities
{
    public class Song
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public int DurationInSeconds { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public string Genre { get; private set; } = null!;
        public int AlbumId { get; private set; }
        public Album Album { get; private set; } = null!;
        
        private Song() { } // For EF Core
        
        public Song(string title, int durationInSeconds, DateTime releaseDate, string genre, int albumId)
        {
            Title = title;
            DurationInSeconds = durationInSeconds;
            ReleaseDate = releaseDate;
            Genre = genre;
            AlbumId = albumId;
        }
    }
}