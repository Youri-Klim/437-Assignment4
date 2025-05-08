using MusicStreaming.Core.Entities;
using System.Reflection;

namespace MusicStreaming.Core.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateTitle(this Album album, string title)
        {
            var property = typeof(Album).GetProperty("Title");
            property?.SetValue(album, title);
        }
        
        public static void UpdateReleaseYear(this Album album, int releaseYear)
        {
            var property = typeof(Album).GetProperty("ReleaseYear");
            property?.SetValue(album, releaseYear);
        }
        
        public static void UpdateGenre(this Album album, string genre)
        {
            var property = typeof(Album).GetProperty("Genre");
            property?.SetValue(album, genre);
        }
        
        public static void UpdateName(this Artist artist, string name)
        {
            var property = typeof(Artist).GetProperty("Name");
            property?.SetValue(artist, name);
        }
        
        public static void UpdateGenre(this Artist artist, string genre)
        {
            var property = typeof(Artist).GetProperty("Genre");
            property?.SetValue(artist, genre);
        }
        
        public static void UpdateTitle(this Song song, string title)
        {
            var property = typeof(Song).GetProperty("Title");
            property?.SetValue(song, title);
        }
        
        public static void UpdateDuration(this Song song, int duration)
        {
            var property = typeof(Song).GetProperty("Duration");
            property?.SetValue(song, duration);
        }
        
        public static void UpdateReleaseDate(this Song song, DateTime releaseDate)
        {
            var property = typeof(Song).GetProperty("ReleaseDate");
            property?.SetValue(song, releaseDate);
        }
        
        public static void UpdateGenre(this Song song, string genre)
        {
            var property = typeof(Song).GetProperty("Genre");
            property?.SetValue(song, genre);
        }
        
        public static void UpdateTitle(this Playlist playlist, string title)
        {
            var property = typeof(Playlist).GetProperty("Title");
            property?.SetValue(playlist, title);
        }
        
        public static void UpdateUserDetails(this User user, string username, string email)
{
    var usernameProperty = typeof(User).GetProperty("Username");
    var emailProperty = typeof(User).GetProperty("Email");
    
    usernameProperty?.SetValue(user, username);
    emailProperty?.SetValue(user, email);
}
    }
}