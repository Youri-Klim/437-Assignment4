using MusicStreaming.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<string> CreateAsync(string username, string email, string password, DateTime dateOfBirth);
        Task UpdateAsync(User user);
        Task DeleteAsync(string id);
    }
}