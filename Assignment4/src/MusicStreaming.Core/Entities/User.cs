namespace MusicStreaming.Core.Entities
{
    public class User
    {

        
        public string? Id { get; private set; }
        public string? Username { get; private set; }
        public string? Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        private readonly List<Playlist> _playlists = new();
        public IReadOnlyCollection<Playlist> Playlists => _playlists.AsReadOnly();
        
        private User() { } // For EF Core
        
        public User(string id, string username, string email, DateTime dateOfBirth)
        {
            Id = id;
            Username = username;
            Email = email;
            DateOfBirth = dateOfBirth;
        }
    }
}