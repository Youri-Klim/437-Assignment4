using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MusicStreamingDbContext _context;

        public UserRepository(MusicStreamingDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Playlists)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<string> CreateAsync(string username, string email, string password, DateTime dateOfBirth)
        {
            var id = Guid.NewGuid().ToString();
            var user = new User(id, username, email, password, dateOfBirth);
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            return id;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}