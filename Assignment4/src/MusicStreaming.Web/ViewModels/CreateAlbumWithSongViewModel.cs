using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.Models
{
    public class CreateAlbumWithSongViewModel
    {
        // Album properties
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        [Range(1900, 2100)]
        public int ReleaseYear { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }
        
        [Required]
        public int ArtistId { get; set; }
        
        // Initial song properties
        [Required]
        [MaxLength(100)]
        public string SongTitle { get; set; }
        
        [Required]
        [Range(1, 1800)]
        public int SongDurationInSeconds { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string SongGenre { get; set; }
    }
}