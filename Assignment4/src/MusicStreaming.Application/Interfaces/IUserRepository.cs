using MusicStreaming.Application.DTOs;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto> GetByIdAsync(string id);
        Task<UserDto> GetByUsernameAsync(string username);
        Task<string> CreateAsync(string username, string email, string password);
        Task UpdateAsync(UserDto user);
    }
}