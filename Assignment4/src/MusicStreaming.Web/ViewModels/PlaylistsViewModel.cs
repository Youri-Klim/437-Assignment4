using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.ViewModels
{
    public class PlaylistViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
    }
    
    public class EditPlaylistViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public List<SongViewModel> Songs { get; set; } = new List<SongViewModel>();
    }
    
    public class CreatePlaylistViewModel
    {
        [Required]
        [MaxLength(100)]
        public required string Title { get; set; }
        public required string UserId { get; set; }
    }
}