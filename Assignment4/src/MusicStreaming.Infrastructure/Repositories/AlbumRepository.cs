using Microsoft.EntityFrameworkCore;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MusicStreamingDbContext _context;

        public AlbumRepository(MusicStreamingDbContext context)
        {
            _context = context;
        }

        public async Task<Album?> GetByIdAsync(int id)
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<Album?> GetWithSongsAsync(int id)
        {
            return await _context.Albums
                .Include(a => a.Songs)
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IReadOnlyList<Album>> ListAllAsync()
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Album>> GetByArtistIdAsync(int artistId)
        {
            return await _context.Albums
                .Where(a => a.ArtistId == artistId)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            return album.Id;
        }

        public async Task UpdateAsync(Album album)
        {
            _context.Entry(album).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}