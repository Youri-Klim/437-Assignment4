namespace MusicStreaming.Core.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get;  set; } = null!;
        public int ReleaseYear { get;  set; }
        public string Genre { get;  set; } = null!;
        public int ArtistId { get;  set; }
        public Artist Artist { get;  set; } = null!;
        private readonly List<Song> _songs = new();
        public IReadOnlyCollection<Song> Songs => _songs.AsReadOnly();
        
        public Album() { }
        
        public Album(string title, int releaseYear, string genre, int artistId)
        {
            Title = title;
            ReleaseYear = releaseYear;
            Genre = genre;
            ArtistId = artistId;
        }
    }
}