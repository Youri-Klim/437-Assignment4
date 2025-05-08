using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Exceptions;
using MusicStreaming.Core.Entities;
using MusicStreaming.Core.Extensions;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Services
{
    public class ArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly ArtistDomainService _artistDomainService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateArtistDto> _createValidator;
        private readonly IValidator<UpdateArtistDto> _updateValidator;

        public ArtistService(
            IArtistRepository artistRepository,
            ArtistDomainService artistDomainService,
            IMapper mapper,
            IValidator<CreateArtistDto> createValidator,
            IValidator<UpdateArtistDto> updateValidator)
        {
            _artistRepository = artistRepository;
            _artistDomainService = artistDomainService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<ArtistDto?> GetByIdAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            return _mapper.Map<ArtistDto>(artist);
        }

        public async Task<ArtistDto?> GetWithAlbumsAsync(int id)
        {
            var artist = await _artistRepository.GetWithAlbumsAsync(id);
    
            // Debug logging
            if (artist != null)
            {
                // Log the album count
                Console.WriteLine($"Artist {artist.Name} has {artist.Albums.Count} albums");
            }
    
            return _mapper.Map<ArtistDto>(artist);
        }

        public async Task<IReadOnlyList<ArtistDto>> ListAllAsync()
        {
            var artists = await _artistRepository.ListAllAsync();
            return _mapper.Map<IReadOnlyList<ArtistDto>>(artists);
        }

        public async Task<int> AddAsync(CreateArtistDto artistDto)
        {
            // FluentValidation
            var validationResult = await _createValidator.ValidateAsync(artistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var artist = new Artist(artistDto.Name, artistDto.Genre);
            
            // Domain validation - convert to FluentValidation format
            var domainErrors = _artistDomainService.ValidateArtist(artist);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            return await _artistRepository.AddAsync(artist);
        }

        public async Task<int> UpdateAsync(UpdateArtistDto artistDto)
        {
            // FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(artistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var artist = await _artistRepository.GetByIdAsync(artistDto.Id);
            if (artist == null)
                throw new NotFoundException($"Artist with ID {artistDto.Id} not found");
                
            // Update properties
            artist.UpdateName(artistDto.Name);
            artist.UpdateGenre(artistDto.Genre);
            
            // Domain validation - convert to FluentValidation format
            var domainErrors = _artistDomainService.ValidateArtist(artist);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            await _artistRepository.UpdateAsync(artist);
            return artist.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null)
                return;
                
            // Check if artist is prolific - might have additional business rules for deletion
            if (_artistDomainService.IsProlificArtist(artist))
            {
                throw new BusinessRuleException("Cannot delete a prolific artist with extensive catalog");
            }
            
            await _artistRepository.DeleteAsync(id);
        }
        
        public async Task<string> GetArtistTierAsync(int id)
        {
            var artist = await _artistRepository.GetWithAlbumsAsync(id);
            if (artist == null)
                return "Unknown";
                
            return _artistDomainService.DetermineArtistTier(artist);
        }
        
        public async Task<bool> IsVerifiedArtistAsync(int id)
        {
            var artist = await _artistRepository.GetWithAlbumsAsync(id);
            if (artist == null)
                return false;
                
            return _artistDomainService.IsVerifiedArtist(artist);
        }

        public async Task<IReadOnlyList<ArtistDto>> GetArtistsWithAlbumsAsync()
{
    var artists = await _artistRepository.GetAllWithAlbumsAsync();
    return _mapper.Map<IReadOnlyList<ArtistDto>>(artists);
}
    }
}