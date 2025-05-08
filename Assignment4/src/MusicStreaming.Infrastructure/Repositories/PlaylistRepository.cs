using Microsoft.EntityFrameworkCore;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly MusicStreamingDbContext _context;

        public PlaylistRepository(MusicStreamingDbContext context)
        {
            _context = context;
        }

        public async Task<Playlist?> GetByIdAsync(int id)
        {
            return await _context.Playlists.FindAsync(id);
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

        public async Task<IReadOnlyList<Playlist>> GetByUserIdAsync(string userId)
        {
            return await _context.Playlists
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> AddAsync(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
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
            var exists = await _context.PlaylistSongs
                .AnyAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);
                
            if (!exists)
            {
                var playlistSong = new PlaylistSong { PlaylistId = playlistId, SongId = songId };
                _context.PlaylistSongs.Add(playlistSong);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveSongAsync(int playlistId, int songId)
        {
            var playlistSong = await _context.PlaylistSongs
                .FirstOrDefaultAsync(ps => ps.PlaylistId == playlistId && ps.SongId == songId);
                
            if (playlistSong != null)
            {
                _context.PlaylistSongs.Remove(playlistSong);
                await _context.SaveChangesAsync();
            }
        }
    }
}