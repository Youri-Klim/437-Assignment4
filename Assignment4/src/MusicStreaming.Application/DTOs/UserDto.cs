using System;
using System.Collections.Generic;

namespace MusicStreaming.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredDate { get; set; }
        public IList<PlaylistDto> Playlists { get; set; } = new List<PlaylistDto>();
    }
}