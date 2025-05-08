using System;

namespace MusicStreaming.Core.Events
{
    public class PlaylistCreatedEvent : DomainEvent
    {
        public int PlaylistId { get; }
        public string PlaylistTitle { get; }
        public string UserId { get; }
        
        public PlaylistCreatedEvent(int playlistId, string playlistTitle, string userId)
        {
            PlaylistId = playlistId;
            PlaylistTitle = playlistTitle;
            UserId = userId;
        }
    }
}