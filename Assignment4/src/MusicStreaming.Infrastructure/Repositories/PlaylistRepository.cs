using Microsoft.EntityFrameworkCore;
using MusicStreaming.Core.Entities;
using MusicStreaming.Core.Interfaces;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly MusicStreamingDbContext _context;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public PlaylistRepository(MusicStreamingDbContext context, IDomainEventDispatcher domainEventDispatcher)
        {
            _context = context;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Playlist?> GetByIdAsync(int id)
        {
            return await _context.Playlists.FindAsync(id);
        }

        public async Task<IReadOnlyList<Playlist>> GetByUserIdAsync(string userId)
        {
            return await _context.Playlists
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<Playlist?> GetWithSongsAsync(int id)
        {
            return await _context.Playlists
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Album)
                            .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Playlist>> ListAllAsync()
        {
            return await _context.Playlists.ToListAsync();
        }

        public async Task<int> AddAsync(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            
            foreach (var domainEvent in playlist.DomainEvents)
            {
                await _domainEventDispatcher.DispatchAsync(domainEvent);
            }
            
            playlist.ClearDomainEvents();
            
            return playlist.Id;
        }

        public async Task UpdateAsync(Playlist playlist)
        {
            _context.Entry(playlist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var playlist = await _context.Playlists.FindAsync(id);
            if (playlist != null)
            {
                _context.Playlists.Remove(playlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddSongAsync(int playlistId, int songId)
        {
            var playlist = await _context.Playlists.FindAsync(playlistId);
            if (playlist != null)
            {
                playlist.AddSong(songId);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveSongAsync(int playlistId, int songId)
        {
            var playlist = await _context.Playlists.FindAsync(playlistId);
            if (playlist != null)
            {
                playlist.RemoveSong(songId);
                await _context.SaveChangesAsync();
            }
        }
    }
}