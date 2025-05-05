using System;
using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.Models
{
    public class SongViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DurationInSeconds { get; set; }
        public string DurationFormatted { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public string ArtistName { get; set; }
    }
    
    public class SongDetailViewModel : SongViewModel
    {
        // Additional properties for details view if needed
    }
    
    public class CreateSongViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        [Range(1, 1800)]
        public int DurationInSeconds { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }
        
        [Required]
        public int AlbumId { get; set; }
    }
    
    public class EditSongViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        [Range(1, 1800)]
        public int DurationInSeconds { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }
        
        [Required]
        public int AlbumId { get; set; }
    }
}