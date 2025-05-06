using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.ViewModels
{
    public class ArtistViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Genre { get; set; }
        public List<AlbumViewModel> Albums { get; set; } = new List<AlbumViewModel>();
        public int AlbumCount { get; set; }
    }
    
    public class CreateArtistViewModel
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Genre { get; set; }
    }
    
    public class EditArtistViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Genre { get; set; }
    }

    public class ArtistDetailViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Genre { get; set; }
        public List<AlbumViewModel> Albums { get; set; } = new List<AlbumViewModel>();
}
}