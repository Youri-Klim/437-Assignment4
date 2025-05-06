namespace MusicStreaming.Core.Entities
{
    public class PlaylistSong
    {
        public int PlaylistId { get;  set; }
        public Playlist Playlist { get;  set; } = null!;
        public int SongId { get;  set; }
        public Song Song { get;  set; } = null!;
        
        public PlaylistSong() { } // For EF Core
        
        public PlaylistSong(int playlistId, int songId)
        {
            PlaylistId = playlistId;
            SongId = songId;
        }
    }
}