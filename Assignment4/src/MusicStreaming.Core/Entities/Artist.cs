namespace MusicStreaming.Core.Entities
{
    public class Artist
    {
        public int Id { get;  set; }
        public string Name { get;  set; } = null!;
        public string Genre { get;  set; } = null!;
        public virtual ICollection<Album> Albums { get; private set; } = new List<Album>();
        
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