namespace MusicStreaming.Core.Entities
{
    public class User
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        
        public User() { }
        
        public User(string id, string username, string email, string password, DateTime dateOfBirth)
        {
            Id = id;
            Username = username;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
        }
        
        public User(string id, string username, string email, DateTime dateOfBirth)
        {
            Id = id;
            Username = username;
            Email = email;
            DateOfBirth = dateOfBirth;
        }
    }
}