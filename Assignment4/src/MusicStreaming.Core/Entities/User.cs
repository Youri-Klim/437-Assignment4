namespace MusicStreaming.Core.Entities
{
    public class User
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        // Add Password property
        public string? Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        // Replace private collection with public collection for EF Core
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        
        public User() { } // For EF Core
        
        // Update constructor to include Password
        public User(string id, string username, string email, string password, DateTime dateOfBirth)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
        }
        
        // Keep the old constructor for backward compatibility
        public User(string id, string username, string email, DateTime dateOfBirth)
        {
            Id = id;
            Username = username;
            Email = email;
            DateOfBirth = dateOfBirth;
        }
    }
}