using MusicStreaming.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Interfaces.Repositories
{
    public interface IPlaylistRepository
    {
        Task<PlaylistDto> GetByIdAsync(int id);
        Task<IReadOnlyList<PlaylistDto>> GetByUserIdAsync(string userId);
        Task<int> AddAsync(CreatePlaylistDto playlist);
        Task UpdateAsync(UpdatePlaylistDto playlist);
        Task DeleteAsync(int id);
        Task AddSongAsync(int playlistId, int songId);
        Task RemoveSongAsync(int playlistId, int songId);
    }
}