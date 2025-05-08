using MusicStreaming.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByUsernameAsync(string username);
        Task<IReadOnlyList<User>> ListAllAsync();
        Task<string> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }
}