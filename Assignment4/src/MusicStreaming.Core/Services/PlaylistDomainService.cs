using MusicStreaming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicStreaming.Core.Services
{
    public class PlaylistDomainService
    {
        private const int MaxSongsPerPlaylist = 500;
        
        public bool CanAddSongToPlaylist(Playlist playlist, Song song)
        {
            // Business rule: Check if playlist has reached max song limit
            return playlist.PlaylistSongs == null || playlist.PlaylistSongs.Count < MaxSongsPerPlaylist;
        }
        
        public bool HasSong(Playlist playlist, int songId)
        {
            // Business rule: Check if song is already in the playlist
            return playlist.PlaylistSongs != null && 
                   playlist.PlaylistSongs.Any(ps => ps.SongId == songId);
        }
        
        public int CalculateTotalDuration(Playlist playlist)
        {
            // Business rule: Calculate total playlist duration in seconds
            if (playlist.PlaylistSongs == null || !playlist.PlaylistSongs.Any())
                return 0;
                
            return playlist.PlaylistSongs.Sum(ps => ps.Song?.Duration ?? 0);
        }
        
        public string GetPlaylistStatistics(Playlist playlist)
        {
            // Business rule: Generate playlist statistics
            if (playlist.PlaylistSongs == null || !playlist.PlaylistSongs.Any())
                return "Empty playlist";
                
            var totalDuration = CalculateTotalDuration(playlist);
            var durationFormatted = TimeSpan.FromSeconds(totalDuration).ToString(@"hh\:mm\:ss");
            var songCount = playlist.PlaylistSongs.Count;
            
            return $"{songCount} songs, {durationFormatted} total time";
        }
        
        public List<string> ValidatePlaylist(Playlist playlist)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(playlist.Title))
                errors.Add("Playlist title cannot be empty");
                
            if (playlist.Title.Length > 100)
                errors.Add("Playlist title cannot exceed 100 characters");
                
            if (string.IsNullOrWhiteSpace(playlist.UserId))
                errors.Add("Playlist must belong to a user");
                
            return errors;
        }
    }
}