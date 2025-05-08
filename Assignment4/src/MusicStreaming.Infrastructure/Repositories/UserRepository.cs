using Microsoft.EntityFrameworkCore;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IReadOnlyList<User>> ListAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<string> AddAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
                
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            
            // Ensure the ID is not null before returning
            return user.Id ?? throw new InvalidOperationException("User ID was not generated properly");
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
                
            _context.Entry(user).State = EntityState.Modified;
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

        public async Task<User?> GetWithPlaylistsAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Playlists)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}