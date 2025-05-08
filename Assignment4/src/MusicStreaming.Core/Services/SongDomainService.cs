using MusicStreaming.Core.Entities;
using System;
using System.Collections.Generic;

namespace MusicStreaming.Core.Services
{
    public class SongDomainService
    {
        public bool IsNewRelease(Song song)
        {
            return (DateTime.Now - song.ReleaseDate).TotalDays < 30;
        }
        
        public bool IsPopular(Song song)
{
    if (song.Album?.ReleaseYear <= DateTime.Now.Year - 25)
        return true;
    
    return false;
    
}
        
        public string FormatDuration(Song song)
        {
            TimeSpan time = TimeSpan.FromSeconds(song.Duration);
            return time.ToString(@"mm\:ss");
        }
        
        public string CategorizeSong(Song song)
        {
            if (song.Duration < 180) 
                return "Short";
            else if (song.Duration >= 180 && song.Duration <= 300)
                return "Standard";
            else if (song.Duration > 300 && song.Duration <= 480)
                return "Extended";
            else
                return "Epic"; 
        }
        
        public List<string> ValidateSong(Song song)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(song.Title))
                errors.Add("Song title cannot be empty");
                
            if (song.Duration <= 0)
                errors.Add("Song duration must be greater than 0 seconds");
                
            if (song.ReleaseDate > DateTime.Now)
                errors.Add("Release date cannot be in the future");
                
            if (string.IsNullOrWhiteSpace(song.Genre))
                errors.Add("Song genre cannot be empty");
                
            return errors;
        }
    }
}