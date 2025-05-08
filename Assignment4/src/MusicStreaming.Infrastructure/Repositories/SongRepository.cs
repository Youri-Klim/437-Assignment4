using Microsoft.EntityFrameworkCore;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly MusicStreamingDbContext _context;

        public SongRepository(MusicStreamingDbContext context)
        {
            _context = context;
        }

        public async Task<Song?> GetByIdAsync(int id)
        {
            return await _context.Songs.FindAsync(id);
        }

        public async Task<IReadOnlyList<Song>> ListAllAsync()
        {
            return await _context.Songs.ToListAsync();
        }

        public async Task<IReadOnlyList<Song>> GetByAlbumIdAsync(int albumId)
        {
            return await _context.Songs
                .Where(s => s.AlbumId == albumId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Song>> SearchAsync(string searchTerm)
        {
            return await _context.Songs
                .Where(s => s.Title.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<int> AddAsync(Song song)
        {
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            return song.Id;
        }

        public async Task UpdateAsync(Song song)
        {
            _context.Entry(song).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }
    }
}