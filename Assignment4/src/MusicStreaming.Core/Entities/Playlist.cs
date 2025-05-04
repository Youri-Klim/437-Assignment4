namespace MusicStreaming.Core.Entities
{
    public class Playlist
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = null!;
        public DateTime CreationDate { get; private set; }
        public string UserId { get; private set; } = null!;
        public User User { get; private set; } = null!;
        private readonly List<PlaylistSong> _playlistSongs = new();
        public IReadOnlyCollection<PlaylistSong> PlaylistSongs => _playlistSongs.AsReadOnly();
        
        private Playlist() { } // For EF Core
        
        public Playlist(string title, string userId)
        {
            Title = title;
            UserId = userId;
            CreationDate = DateTime.UtcNow;
        }
        
        public void AddSong(int songId)
        {
            if (!_playlistSongs.Any(ps => ps.SongId == songId))
            {
                _playlistSongs.Add(new PlaylistSong(Id, songId));
            }
        }
        
        public void RemoveSong(int songId)
        {
            var item = _playlistSongs.FirstOrDefault(ps => ps.SongId == songId);
            if (item != null)
            {
                _playlistSongs.Remove(item);
            }
        }
    }
}