using System;
using System.Collections.Generic;

namespace MusicStreaming.Application.DTOs
{
    public class UserDto
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public DateTime RegisteredDate { get; set; }
        public IList<PlaylistDto> Playlists { get; set; } = new List<PlaylistDto>();
    }
}