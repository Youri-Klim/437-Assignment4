using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.Models
{
    public class ArtistViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public List<AlbumViewModel> Albums { get; set; } = new List<AlbumViewModel>();
        public int AlbumCount { get; set; }
    }
    
    public class CreateArtistViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }
    }
    
    public class EditArtistViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }
    }
}