using Microsoft.EntityFrameworkCore;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MusicStreamingDbContext _context;

        public ArtistRepository(MusicStreamingDbContext context)
        {
            _context = context;
        }

        public async Task<Artist?> GetByIdAsync(int id)
        {
            return await _context.Artists.FindAsync(id);
        }

        public async Task<Artist?> GetWithAlbumsAsync(int id)
        {
            return await _context.Artists
                .Include(a => a.Albums)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IReadOnlyList<Artist>> GetAllWithAlbumsAsync()
    {
        return await _context.Artists
                .Include(a => a.Albums)
             .ToListAsync();
    }

        public async Task<IReadOnlyList<Artist>> ListAllAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<int> AddAsync(Artist artist)
        {
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            return artist.Id;
        }

        public async Task UpdateAsync(Artist artist)
        {
            _context.Entry(artist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            if (artist != null)
            {
                _context.Artists.Remove(artist);
                await _context.SaveChangesAsync();
            }
        }
    }
}