using System;
using System.Collections.Generic;

namespace MusicStreaming.Application.DTOs
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public IList<SongDto> Songs { get; set; } = new List<SongDto>();
    }
}