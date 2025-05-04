using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly MusicStreamingDbContext _context;
        private readonly IMapper _mapper;
        
        public SongRepository(MusicStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SongDto> GetByIdAsync(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Album)
                .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(s => s.Id == id);
                
            return _mapper.Map<SongDto>(song);
        }
        
        public async Task<IReadOnlyList<SongDto>> ListAllAsync()
        {
            var songs = await _context.Songs
                .Include(s => s.Album)
                .ThenInclude(a => a.Artist)
                .ToListAsync();
                
            return _mapper.Map<IReadOnlyList<SongDto>>(songs);
        }
        
        public async Task<IReadOnlyList<SongDto>> GetByAlbumIdAsync(int albumId)
        {
            var songs = await _context.Songs
                .Include(s => s.Album)
                .ThenInclude(a => a.Artist)
                .Where(s => s.AlbumId == albumId)
                .ToListAsync();
                
            return _mapper.Map<IReadOnlyList<SongDto>>(songs);
        }
        
        public async Task<int> AddAsync(CreateSongDto songDto)
        {
            var song = new Song(
                songDto.Title,
                songDto.DurationInSeconds,
                songDto.ReleaseDate,
                songDto.Genre,
                songDto.AlbumId);
                
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();
            
            return song.Id;
        }
        
        public async Task UpdateAsync(UpdateSongDto songDto)
        {
            var song = await _context.Songs.FindAsync(songDto.Id);
            if (song == null) return;
            
            // Update song properties - typically you'd have a method in the entity
            // for this but since we don't, we'll update via EF Core
            
            _context.Entry(song).CurrentValues.SetValues(new
            {
                songDto.Title,
                songDto.DurationInSeconds,
                songDto.ReleaseDate,
                songDto.Genre,
                songDto.AlbumId
            });
            
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