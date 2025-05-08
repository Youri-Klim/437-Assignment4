using MusicStreaming.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Interfaces.Repositories
{
    public interface IArtistRepository
    {
        Task<Artist?> GetByIdAsync(int id);
        Task<Artist?> GetWithAlbumsAsync(int id);
        Task<IReadOnlyList<Artist>> ListAllAsync();
        Task<IReadOnlyList<Artist>> GetAllWithAlbumsAsync(); // Add this method
        Task<int> AddAsync(Artist artist);
        Task UpdateAsync(Artist artist);
        Task DeleteAsync(int id);
    }
}