using MusicStreaming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MusicStreaming.Core.Services
{
    public class UserDomainService
    {
        public bool IsPremiumUser(User user)
        {
            // Simplified implementation since you don't have a Subscription property
            // You can implement this based on your actual business rules
            return false; // Default to free tier for now
        }
        
        public int GetMaxAllowedPlaylists(User user)
        {
            // Business rule: All users limited to 10 playlists for now
            // You can enhance this when you implement subscription features
            return 10;
        }
        
        public bool CanCreateNewPlaylist(User user)
        {
            // Business rule: Check if user can create a new playlist
            return user.Playlists == null || user.Playlists.Count < GetMaxAllowedPlaylists(user);
        }
        
        public string DetermineUserLevel(User user)
        {
            // Business rule: Determine user level based on activity
            if (user.Playlists == null)
                return "Newbie";
                
            int totalSongs = 0;
            foreach (var playlist in user.Playlists)
            {
                if (playlist.PlaylistSongs != null)
                    totalSongs += playlist.PlaylistSongs.Count;
            }
            
            if (totalSongs > 500)
                return "Audiophile";
            else if (totalSongs > 100)
                return "Music Lover";
            else if (totalSongs > 20)
                return "Casual Listener";
            else
                return "Newbie";
        }
        
        public List<string> ValidateUser(User user)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(user.Username))
                errors.Add("Username cannot be empty");
            else if (user.Username.Length < 3 || user.Username.Length > 20)
                errors.Add("Username must be between 3 and 20 characters");
                
            if (string.IsNullOrWhiteSpace(user.Email))
                errors.Add("Email cannot be empty");
            else if (!IsValidEmail(user.Email))
                errors.Add("Email format is invalid");
                
            return errors;
        }
        
        private bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
                
            try
            {
                // Simple regex for email validation
                var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
                return regex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }
    }
}