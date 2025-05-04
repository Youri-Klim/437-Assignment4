using MusicStreaming.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Interfaces.Repositories
{
    public interface IAlbumRepository
    {
        Task<AlbumDto> GetByIdAsync(int id);
        Task<IReadOnlyList<AlbumDto>> ListAllAsync();
        Task<int> AddAsync(CreateAlbumDto album);
        Task UpdateAsync(UpdateAlbumDto album);
        Task DeleteAsync(int id);
    }
}