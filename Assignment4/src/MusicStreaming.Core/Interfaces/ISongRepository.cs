using MusicStreaming.Core.Entities;
using MusicStreaming.Core.Common;
namespace MusicStreaming.Core.Interfaces // or the correct namespace where Song is defined
{
    public interface ISongRepository
    {
        Task<Song> GetByIdAsync(int id);
        Task<List<Song>> GetAllAsync();
        Task<List<Song>> GetSongsByAlbumAsync(int albumId);
        Task<List<Song>> GetSongsByArtistAsync(int artistId);
        Task<PagedResult<Song>> GetPagedSongsAsync(int pageNumber, int pageSize, string searchTerm, string genre, string sortBy, bool ascending);
        Task<Song> AddAsync(Song song);
        Task UpdateAsync(Song song);
        Task DeleteAsync(int id);
    }
}