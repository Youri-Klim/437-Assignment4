namespace MusicStreaming.Core.Entities
{
    public class Artist
    {
        public int Id { get;  set; }
        public string Name { get;  set; } = null!;
        public string Genre { get;  set; } = null!;
        private readonly List<Album> _albums = new();
        public IReadOnlyCollection<Album> Albums => _albums.AsReadOnly();
        
        public Artist() { } // For EF Core
        
        public Artist(string name, string genre)
        {
            Name = name;
            Genre = genre;
        }
        
        public void UpdateDetails(string name, string genre)
        {
            Name = name;
            Genre = genre;
        }

        
    }
}