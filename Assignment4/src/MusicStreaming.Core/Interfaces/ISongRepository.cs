using MusicStreaming.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Interfaces.Repositories
{
    public interface ISongRepository
    {
        Task<Song?> GetByIdAsync(int id);
        Task<IReadOnlyList<Song>> ListAllAsync();
        Task<IReadOnlyList<Song>> GetByAlbumIdAsync(int albumId);
        Task<int> AddAsync(Song song);
        Task UpdateAsync(Song song);
        Task DeleteAsync(int id);
        Task<IReadOnlyList<Song>> SearchAsync(string searchTerm);
    }
}