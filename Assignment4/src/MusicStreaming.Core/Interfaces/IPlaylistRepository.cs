using MusicStreaming.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Interfaces.Repositories
{
    public interface IPlaylistRepository
    {
        Task<Playlist?> GetByIdAsync(int id);
        Task<Playlist?> GetWithSongsAsync(int id);
        Task<IReadOnlyList<Playlist>> ListAllAsync();
        Task<IReadOnlyList<Playlist>> GetByUserIdAsync(string userId);
        Task<int> AddAsync(Playlist playlist);
        Task UpdateAsync(Playlist playlist);
        Task DeleteAsync(int id);
        Task AddSongAsync(int playlistId, int songId);
        Task RemoveSongAsync(int playlistId, int songId);
    }
}