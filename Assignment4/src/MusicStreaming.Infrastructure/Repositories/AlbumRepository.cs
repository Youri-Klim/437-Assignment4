using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MusicStreamingDbContext _context;
        private readonly IMapper _mapper;

        public AlbumRepository(MusicStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AlbumDto> GetByIdAsync(int id)
        {
            var album = await _context.Albums
                .Include(a => a.Artist)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            return _mapper.Map<AlbumDto>(album);
        }

        public async Task<IReadOnlyList<AlbumDto>> ListAllAsync()
        {
            var albums = await _context.Albums
                .Include(a => a.Artist)
                .ToListAsync();
            
            return _mapper.Map<IReadOnlyList<AlbumDto>>(albums);
        }

        public async Task<int> AddAsync(CreateAlbumDto albumDto)
        {
            // Use the parameterized constructor
            var album = new Album(
                albumDto.Title,
                albumDto.ReleaseYear,
                albumDto.Genre,
                albumDto.ArtistId
            );
            
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            
            return album.Id;
        }

        public async Task UpdateAsync(UpdateAlbumDto albumDto)
        {
            var album = await _context.Albums.FindAsync(albumDto.Id);
            if (album != null)
            {
                // Use reflection to update properties with private setters
                var entityType = _context.Entry(album).Entity.GetType();
                var titleProperty = entityType.GetProperty("Title");
                var releaseYearProperty = entityType.GetProperty("ReleaseYear");
                var genreProperty = entityType.GetProperty("Genre");
                var artistIdProperty = entityType.GetProperty("ArtistId");
                
                titleProperty?.SetValue(album, albumDto.Title);
                releaseYearProperty?.SetValue(album, albumDto.ReleaseYear);
                genreProperty?.SetValue(album, albumDto.Genre);
                artistIdProperty?.SetValue(album, albumDto.ArtistId);
                
                await _context.SaveChangesAsync();
            }
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