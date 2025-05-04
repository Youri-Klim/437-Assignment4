namespace MusicStreaming.Core.Entities
{
    public class PlaylistSong
    {
        public int PlaylistId { get; private set; }
        public Playlist Playlist { get; private set; } = null!;
        public int SongId { get; private set; }
        public Song Song { get; private set; } = null!;
        
        private PlaylistSong() { } // For EF Core
        
        public PlaylistSong(int playlistId, int songId)
        {
            PlaylistId = playlistId;
            SongId = songId;
        }
    }
}