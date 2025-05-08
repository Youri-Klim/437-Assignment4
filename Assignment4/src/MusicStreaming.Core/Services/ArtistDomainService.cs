using MusicStreaming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicStreaming.Core.Services
{
    public class ArtistDomainService
    {
        public bool IsVerifiedArtist(Artist artist)
        {
            return artist.Albums != null && artist.Albums.Count >= 3;
        }
        
        public bool IsProlificArtist(Artist artist)
        {
            if (artist.Albums == null) return false;
            
            int totalSongs = artist.Albums.Sum(a => a.Songs?.Count ?? 0);
            return totalSongs > 50;
        }
        
        public string DetermineArtistTier(Artist artist)
        {
            if (artist.Albums == null || artist.Albums.Count == 0)
                return "New";
                
            int totalSongs = artist.Albums.Sum(a => a.Songs?.Count ?? 0);
            
            if (artist.Albums.Count > 10 || totalSongs > 100)
                return "Platinum";
            else if (artist.Albums.Count > 5 || totalSongs > 50)
                return "Gold";
            else if (artist.Albums.Count > 2 || totalSongs > 20)
                return "Silver";
            else
                return "Bronze";
        }
        
        public List<string> ValidateArtist(Artist artist)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(artist.Name))
                errors.Add("Artist name cannot be empty");
                
            if (artist.Name.Length > 100)
                errors.Add("Artist name cannot exceed 100 characters");
                
            if (string.IsNullOrWhiteSpace(artist.Genre))
                errors.Add("Artist genre cannot be empty");
                
            return errors;
        }
    }
}