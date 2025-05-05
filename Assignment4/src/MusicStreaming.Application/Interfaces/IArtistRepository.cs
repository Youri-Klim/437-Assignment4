using MusicStreaming.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Interfaces.Repositories
{
    public interface IArtistRepository
    {
        Task<ArtistDto> GetByIdAsync(int id);
        Task<IReadOnlyList<ArtistDto>> ListAllAsync();
        Task<IReadOnlyList<ArtistDto>> GetArtistsWithAlbumsAsync();
        Task<int> AddAsync(CreateArtistDto artist);
        Task UpdateAsync(UpdateArtistDto artist);
        Task DeleteAsync(int id);
    }
}