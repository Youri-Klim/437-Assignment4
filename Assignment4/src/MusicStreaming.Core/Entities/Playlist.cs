using MusicStreaming.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MusicStreaming.Core.Entities
{
    public class Playlist : Entity
    {
        public int Id { get;  set; }
        public string Title { get;  set; } = null!;
        public DateTime CreationDate { get;  set; }
        public string UserId { get;  set; } = null!;
        public User User { get;  set; } = null!;
        private readonly List<PlaylistSong> _playlistSongs = new();
        public IReadOnlyCollection<PlaylistSong> PlaylistSongs => _playlistSongs.AsReadOnly();
        
        public Playlist() { }
        
        public Playlist(string title, string userId)
        {
            Title = title;
            UserId = userId;
            CreationDate = DateTime.UtcNow;
            AddDomainEvent(new PlaylistCreatedEvent(0, title, userId));
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