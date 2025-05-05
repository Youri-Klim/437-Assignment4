using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure.Data;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MusicStreaming.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MusicStreamingDbContext _context;
        private readonly IMapper _mapper;

        public ArtistRepository(MusicStreamingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArtistDto> GetByIdAsync(int id)
        {
            var artist = await _context.Artists.FindAsync(id);
            return _mapper.Map<ArtistDto>(artist);
        }

        public async Task<IReadOnlyList<ArtistDto>> ListAllAsync()
        {
            var artists = await _context.Artists.ToListAsync();
            return _mapper.Map<IReadOnlyList<ArtistDto>>(artists);
        }

        public async Task<IReadOnlyList<ArtistDto>> GetArtistsWithAlbumsAsync()
        {
            var artists = await _context.Artists
                .Include(a => a.Albums)
                .ToListAsync();
                
            return _mapper.Map<IReadOnlyList<ArtistDto>>(artists);
        }

        public async Task<int> AddAsync(CreateArtistDto artistDto)
        {
            // Use the parameterized constructor
            var artist = new Artist(artistDto.Name, artistDto.Genre);
            
            _context.Artists.Add(artist);
            await _context.SaveChangesAsync();
            
            return artist.Id;
        }

        public async Task UpdateAsync(UpdateArtistDto artistDto)
        {
            var artist = await _context.Artists.FindAsync(artistDto.Id);
            if (artist != null)
            {
                // Use reflection to set properties with private setters
                var entityType = _context.Entry(artist).Entity.GetType();
                var nameProperty = entityType.GetProperty("Name");
                var genreProperty = entityType.GetProperty("Genre");
                
                nameProperty?.SetValue(artist, artistDto.Name);
                genreProperty?.SetValue(artist, artistDto.Genre);
                
                await _context.SaveChangesAsync();
            }
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