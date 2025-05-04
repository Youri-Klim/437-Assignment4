namespace MusicStreaming.Core.Entities
{
    public class Album
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public int ReleaseYear { get; private set; }
        public string Genre { get; private set; } = null!;
        public int ArtistId { get; private set; }
        public Artist Artist { get; private set; } = null!;
        private readonly List<Song> _songs = new();
        public IReadOnlyCollection<Song> Songs => _songs.AsReadOnly();
        
        private Album() { } // For EF Core
        
        public Album(string title, int releaseYear, string genre, int artistId)
        {
            Title = title;
            ReleaseYear = releaseYear;
            Genre = genre;
            ArtistId = artistId;
        }
    }
}