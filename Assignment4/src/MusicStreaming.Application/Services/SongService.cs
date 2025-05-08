using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Exceptions;
using MusicStreaming.Core.Entities;
using MusicStreaming.Core.Extensions;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Services
{
    public class SongService
    {
        private readonly ISongRepository _songRepository;
        private readonly SongDomainService _songDomainService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateSongDto> _createValidator;
        private readonly IValidator<UpdateSongDto> _updateValidator;

        public SongService(
            ISongRepository songRepository,
            SongDomainService songDomainService,
            IMapper mapper,
            IValidator<CreateSongDto> createValidator,
            IValidator<UpdateSongDto> updateValidator)
        {
            _songRepository = songRepository;
            _songDomainService = songDomainService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<SongDto?> GetByIdAsync(int id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            return _mapper.Map<SongDto>(song);
        }

        public async Task<IReadOnlyList<SongDto>> ListAllAsync()
        {
            var songs = await _songRepository.ListAllAsync();
            return _mapper.Map<IReadOnlyList<SongDto>>(songs);
        }

        public async Task<IReadOnlyList<SongDto>> GetByAlbumIdAsync(int albumId)
        {
            var songs = await _songRepository.GetByAlbumIdAsync(albumId);
            return _mapper.Map<IReadOnlyList<SongDto>>(songs);
        }

        public async Task<IReadOnlyList<SongDto>> SearchAsync(string searchTerm)
        {
            var songs = await _songRepository.SearchAsync(searchTerm);
            return _mapper.Map<IReadOnlyList<SongDto>>(songs);
        }

        public async Task<int> AddAsync(CreateSongDto songDto)
        {
            // FluentValidation
            var validationResult = await _createValidator.ValidateAsync(songDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var song = new Song(
                songDto.Title,
                songDto.Duration,
                songDto.ReleaseDate,
                songDto.Genre,
                songDto.AlbumId
            );
            
            // Domain validation
            var domainErrors = _songDomainService.ValidateSong(song);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            return await _songRepository.AddAsync(song);
        }

        public async Task<int> UpdateAsync(UpdateSongDto songDto)
        {
            // FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(songDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var song = await _songRepository.GetByIdAsync(songDto.Id);
            if (song == null)
                throw new NotFoundException($"Song with ID {songDto.Id} not found");
                
            // Update properties
            song.UpdateTitle(songDto.Title);
            song.UpdateDuration(songDto.Duration);
            song.UpdateReleaseDate(songDto.ReleaseDate);
            song.UpdateGenre(songDto.Genre);
            song.AlbumId = songDto.AlbumId;
            
            // Domain validation
            var domainErrors = _songDomainService.ValidateSong(song);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            await _songRepository.UpdateAsync(song);
            return song.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            if (song == null)
                return;
                
            if (_songDomainService.IsPopular(song))
            {
                throw new BusinessRuleException("Cannot delete a popular song with high stream count");
            }
            
            await _songRepository.DeleteAsync(id);
        }
        
        public async Task<string> GetSongCategoryAsync(int id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            if (song == null)
                return "Unknown";
                
            return _songDomainService.CategorizeSong(song);
        }
        
        public async Task<bool> IsNewReleaseAsync(int id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            if (song == null)
                return false;
                
            return _songDomainService.IsNewRelease(song);
        }
        
        public string FormatDuration(int id, int duration)
        {
            return TimeSpan.FromSeconds(duration).ToString(@"mm\:ss");
        }
    }
}