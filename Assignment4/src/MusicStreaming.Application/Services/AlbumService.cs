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
    public class AlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly AlbumDomainService _albumDomainService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAlbumDto> _createValidator;
        private readonly IValidator<UpdateAlbumDto> _updateValidator;

        public AlbumService(
            IAlbumRepository albumRepository,
            AlbumDomainService albumDomainService,
            IMapper mapper,
            IValidator<CreateAlbumDto> createValidator,
            IValidator<UpdateAlbumDto> updateValidator)
        {
            _albumRepository = albumRepository;
            _albumDomainService = albumDomainService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        
        public async Task<AlbumDto?> GetByIdAsync(int id)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            return _mapper.Map<AlbumDto>(album);
        }

        public async Task<AlbumDto?> GetWithSongsAsync(int id)
        {
            var album = await _albumRepository.GetWithSongsAsync(id);
            return _mapper.Map<AlbumDto>(album);
        }

        public async Task<IReadOnlyList<AlbumDto>> ListAllAsync()
        {
            var albums = await _albumRepository.ListAllAsync();
            return _mapper.Map<IReadOnlyList<AlbumDto>>(albums);
        }

        public async Task<IReadOnlyList<AlbumDto>> GetByArtistIdAsync(int artistId)
        {
            var albums = await _albumRepository.GetByArtistIdAsync(artistId);
            return _mapper.Map<IReadOnlyList<AlbumDto>>(albums);
        }

        public async Task<int> AddAsync(CreateAlbumDto albumDto)
        {
            // FluentValidation
            var validationResult = await _createValidator.ValidateAsync(albumDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var album = new Album(
                albumDto.Title,
                albumDto.ReleaseYear,
                albumDto.Genre,
                albumDto.ArtistId
            );
            
            // Domain validation
            var domainErrors = _albumDomainService.ValidateAlbum(album);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            return await _albumRepository.AddAsync(album);
        }

        public async Task<int> UpdateAsync(UpdateAlbumDto albumDto)
        {
            // FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(albumDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var album = await _albumRepository.GetByIdAsync(albumDto.Id);
            if (album == null)
                throw new NotFoundException($"Album with ID {albumDto.Id} not found");
                
            // Update album properties
            album.UpdateTitle(albumDto.Title);
            album.UpdateReleaseYear(albumDto.ReleaseYear);
            album.UpdateGenre(albumDto.Genre);
            album.ArtistId = albumDto.ArtistId;
            
            // Domain validation
            var domainErrors = _albumDomainService.ValidateAlbum(album);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            await _albumRepository.UpdateAsync(album);
            return album.Id;
        }
        
        public async Task DeleteAsync(int id)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            if (album == null)
                return;
            
            await _albumRepository.DeleteAsync(id);
        }
        
        public async Task<bool> IsAlbumClassicAsync(int id)
        {
            var album = await _albumRepository.GetByIdAsync(id);
            if (album == null)
                return false;
                
            return _albumDomainService.IsAlbumClassic(album);
        }
    }
}