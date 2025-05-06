namespace MusicStreaming.Core.Entities
{
    public class Song
    {
        public int Id { get;  set; }
        public string Title { get;  set; } = null!;
        public int Duration { get;  set; }
        public DateTime ReleaseDate { get;  set; }
        public string Genre { get;  set; } = null!;
        public int AlbumId { get;  set; }
        public Album Album { get;  set; } = null!;
        
        public Song() { } // For EF Core
        
        public Song(string title, int duration, DateTime releaseDate, string genre, int albumId)
        {
            Title = title;
            Duration = duration;
            ReleaseDate = releaseDate;
            Genre = genre;
            AlbumId = albumId;
        }
    }
}