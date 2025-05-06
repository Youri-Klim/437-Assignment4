using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.ViewModels
{
    public class AlbumViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
        public required string Genre { get; set; }
        public int ArtistId { get; set; }
        public required string ArtistName { get; set; }
    }
    
    public class AlbumDetailViewModel : AlbumViewModel
    {
        public List<SongViewModel> Songs { get; set; } = new List<SongViewModel>();
    }
    
    public class CreateAlbumViewModel
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        
        [Required]
        [Range(1900, 2100)]
        public int ReleaseYear { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Genre { get; set; }
        
        [Required]
        public int ArtistId { get; set; }
    }
    
    public class EditAlbumViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        
        [Required]
        [Range(1900, 2100)]
        public int ReleaseYear { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Genre { get; set; }
        
        [Required]
        public int ArtistId { get; set; }
    }
}