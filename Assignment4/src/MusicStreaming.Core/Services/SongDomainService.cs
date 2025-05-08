using MusicStreaming.Core.Entities;
using System;
using System.Collections.Generic;

namespace MusicStreaming.Core.Services
{
    public class SongDomainService
    {
        public bool IsNewRelease(Song song)
        {
            // Business rule: A song is considered a new release if it's less than 30 days old
            return (DateTime.Now - song.ReleaseDate).TotalDays < 30;
        }
        
        public bool IsPopular(Song song)
{
    // Simplified business rule that doesn't rely on stream count
    // For example, songs in classic albums might be considered popular
    if (song.Album?.ReleaseYear <= DateTime.Now.Year - 25)
        return true;
    
    // Or we could consider all songs popular by default until we have stream data
    return false;
    
    // Future implementation when StreamCount is added:
    // return song.StreamCount > 1000000;
}
        
        public string FormatDuration(Song song)
        {
            // Business rule: Format duration as mm:ss
            TimeSpan time = TimeSpan.FromSeconds(song.Duration);
            return time.ToString(@"mm\:ss");
        }
        
        public string CategorizeSong(Song song)
        {
            // Business rule: Categorize songs by length
            if (song.Duration < 180) // Less than 3 minutes
                return "Short";
            else if (song.Duration >= 180 && song.Duration <= 300) // 3-5 minutes
                return "Standard";
            else if (song.Duration > 300 && song.Duration <= 480) // 5-8 minutes
                return "Extended";
            else
                return "Epic"; // > 8 minutes
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