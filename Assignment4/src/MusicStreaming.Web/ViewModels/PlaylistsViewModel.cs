using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicStreaming.Web.Models
{
    public class PlaylistViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
    
    public class EditPlaylistViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<SongViewModel> Songs { get; set; } = new List<SongViewModel>();
    }
    
    public class CreatePlaylistViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}