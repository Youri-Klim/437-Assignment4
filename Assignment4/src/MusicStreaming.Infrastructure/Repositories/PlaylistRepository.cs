using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly MusicStreamingDbContext _context;
        private readonly IMapper _mapper;

        public PlaylistRepository(MusicStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PlaylistDto> GetByIdAsync(int id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            return _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<PlaylistDto> GetWithSongsAsync(int id)
        {
            var playlist = await _context.Playlists
                .Include(p => p.User)
                .Include(p => p.PlaylistSongs)
                    .ThenInclude(ps => ps.Song)
                        .ThenInclude(s => s.Album)
                            .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            return _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<IReadOnlyList<PlaylistDto>> ListAllAsync()
        {
            var playlists = await _context.Playlists
                .Include(p => p.User)
                .ToListAsync();
                
            return _mapper.Map<IReadOnlyList<PlaylistDto>>(playlists);
        }

        public async Task<IReadOnlyList<PlaylistDto>> GetByUserIdAsync(string userId)
        {
            var playlists = await _context.Playlists
                .Where(p => p.UserId == userId)
                .Include(p => p.User)
                .ToListAsync();
                
            return _mapper.Map<IReadOnlyList<PlaylistDto>>(playlists);
        }

        public async Task<int> AddAsync(CreatePlaylistDto playlistDto)
        {
            // Use the parameterized constructor
            var playlist = new Playlist(playlistDto.Title, playlistDto.UserId);
            
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
            
            return playlist.Id;
        }

        public async Task UpdateAsync(UpdatePlaylistDto playlistDto)
        {
            var playlist = await _context.Playlists.FindAsync(playlistDto.Id);
            if (playlist != null)
            {
                // Use reflection to update property with private setter
                var entityType = _context.Entry(playlist).Entity.GetType();
                var titleProperty = entityType.GetProperty("Title");
                
                titleProperty?.SetValue(playlist, playlistDto.Title);
                
                await _context.SaveChangesAsync();
            }
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
            // Use parameterized constructor
            var playlistSong = new PlaylistSong(playlistId, songId);
            
            _context.PlaylistSongs.Add(playlistSong);
            await _context.SaveChangesAsync();
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