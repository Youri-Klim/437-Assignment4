using MusicStreaming.Core.Entities;

namespace MusicStreaming.Core.Events
{
    public class SongPlayedEvent : DomainEvent
    {
        public int SongId { get; }
        public int? UserId { get; }
        
        public SongPlayedEvent(int songId, int? userId = null)
        {
            SongId = songId;
            UserId = userId;
        }
    }
}