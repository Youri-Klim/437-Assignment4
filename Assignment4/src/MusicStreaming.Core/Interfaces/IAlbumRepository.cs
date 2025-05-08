using MusicStreaming.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Interfaces.Repositories
{
    public interface IAlbumRepository
    {
        Task<Album?> GetByIdAsync(int id);
        Task<Album?> GetWithSongsAsync(int id);
        Task<IReadOnlyList<Album>> ListAllAsync();
        Task<IReadOnlyList<Album>> GetByArtistIdAsync(int artistId);
        Task<int> AddAsync(Album album);
        Task UpdateAsync(Album album);
        Task DeleteAsync(int id);
    }
}