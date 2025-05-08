using MusicStreaming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicStreaming.Core.Services
{
    public class AlbumDomainService
    {
        public bool IsAlbumComplete(Album album)
        {
            // Business rule: An album is complete if it has at least 3 songs
            return album.Songs != null && album.Songs.Count >= 3;
        }
        
        public bool IsAlbumClassic(Album album)
        {
            // Business rule: Album is a classic if it's more than 25 years old
            return album.ReleaseYear <= DateTime.Now.Year - 25;
        }
        
        public bool CanDeleteAlbum(Album album)
{
    // Business rule: Can't delete classic albums (simpler rule)
    // You can enhance this later when you have stream data
    return !IsAlbumClassic(album);
    
    // Alternatively, calculate something based on songs
    // int estimatedStreams = album.Songs?.Count * 10000 ?? 0;
    // return !IsAlbumClassic(album) || estimatedStreams < 1000000;
}
        
        public List<string> ValidateAlbum(Album album)
        {
            var errors = new List<string>();
            
            if (string.IsNullOrWhiteSpace(album.Title))
                errors.Add("Album title cannot be empty");
                
            if (album.ReleaseYear < 1900 || album.ReleaseYear > DateTime.Now.Year)
                errors.Add("Album release year must be between 1900 and current year");
                
            if (string.IsNullOrWhiteSpace(album.Genre))
                errors.Add("Album genre cannot be empty");
                
            return errors;
        }
    }
}