using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.ViewModels
{
    public class CreateAlbumWithSongViewModel
    {
        // Album properties
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
        
        // Initial song properties
        [Required]
        [MaxLength(100)]
        public required string SongTitle { get; set; }
        
        [Required]
        [Range(1, 1800)]
        public int SongDuration { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string SongGenre { get; set; }
    }
}