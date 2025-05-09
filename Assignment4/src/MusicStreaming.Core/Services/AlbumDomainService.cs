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
            return album.Songs != null && album.Songs.Count >= 3;
        }
        
        public bool IsAlbumClassic(Album album)
        {
            return album.ReleaseYear <= DateTime.Now.Year - 25;
        }
        
        public bool CanDeleteAlbum(Album album)
        {
            return true;
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