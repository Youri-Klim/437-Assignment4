using MusicStreaming.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Interfaces.Repositories
{
    public interface ISongRepository
    {
        Task<SongDto> GetByIdAsync(int id);
        Task<IReadOnlyList<SongDto>> ListAllAsync();
        Task<IReadOnlyList<SongDto>> GetByAlbumIdAsync(int albumId);
        Task<int> AddAsync(CreateSongDto song);
        Task UpdateAsync(UpdateSongDto song);
        Task DeleteAsync(int id);
    }
}