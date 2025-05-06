using System;
using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.ViewModels
{
    public class SongViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Duration { get; set; }
        public required string DurationFormatted { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Genre { get; set; }
        public int AlbumId { get; set; }
        public required string AlbumTitle { get; set; }
        public required string ArtistName { get; set; }
    }
    
    public class SongDetailViewModel : SongViewModel
    {
        // Additional properties for details view if needed
    }
    
    public class CreateSongViewModel
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        
        [Required]
        [Range(1, 1800)]
        public int Duration { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Genre { get; set; }
        
        [Required]
        public int AlbumId { get; set; }
    }
    
    public class EditSongViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        
        [Required]
        [Range(1, 1800)]
        public int Duration { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Genre { get; set; }
        
        [Required]
        public int AlbumId { get; set; }
    }
}