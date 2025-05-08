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
    public class PlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly ISongRepository _songRepository;
        private readonly PlaylistDomainService _playlistDomainService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePlaylistDto> _createValidator;
        private readonly IValidator<UpdatePlaylistDto> _updateValidator;

        public PlaylistService(
            IPlaylistRepository playlistRepository,
            ISongRepository songRepository,
            PlaylistDomainService playlistDomainService,
            IMapper mapper,
            IValidator<CreatePlaylistDto> createValidator,
            IValidator<UpdatePlaylistDto> updateValidator)
        {
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
            _playlistDomainService = playlistDomainService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<PlaylistDto?> GetByIdAsync(int id)
        {
            var playlist = await _playlistRepository.GetByIdAsync(id);
            return _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<PlaylistDto?> GetWithSongsAsync(int id)
        {
            var playlist = await _playlistRepository.GetWithSongsAsync(id);
            return _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<IReadOnlyList<PlaylistDto>> ListAllAsync()
        {
            var playlists = await _playlistRepository.ListAllAsync();
            return _mapper.Map<IReadOnlyList<PlaylistDto>>(playlists);
        }

        public async Task<IReadOnlyList<PlaylistDto>> GetByUserIdAsync(string userId)
        {
            var playlists = await _playlistRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IReadOnlyList<PlaylistDto>>(playlists);
        }

        public async Task<int> AddAsync(CreatePlaylistDto playlistDto)
        {
            // FluentValidation
            var validationResult = await _createValidator.ValidateAsync(playlistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var playlist = new Playlist(playlistDto.Title, playlistDto.UserId);
            
            // Domain validation
            var domainErrors = _playlistDomainService.ValidatePlaylist(playlist);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            return await _playlistRepository.AddAsync(playlist);
        }

        public async Task UpdateAsync(UpdatePlaylistDto playlistDto)
        {
            // FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(playlistDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var playlist = await _playlistRepository.GetByIdAsync(playlistDto.Id);
            if (playlist == null)
                throw new NotFoundException($"Playlist with ID {playlistDto.Id} not found");
                
            // Update properties
            playlist.UpdateTitle(playlistDto.Title);
            
            // Domain validation
            var domainErrors = _playlistDomainService.ValidatePlaylist(playlist);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            await _playlistRepository.UpdateAsync(playlist);
        }

        public async Task DeleteAsync(int id)
        {
            // Verify the playlist exists
            var playlist = await _playlistRepository.GetByIdAsync(id);
            if (playlist == null)
                throw new NotFoundException($"Playlist with ID {id} not found");
                
            await _playlistRepository.DeleteAsync(id);
        }
        
        public async Task AddSongToPlaylistAsync(int playlistId, int songId)
        {
            var playlist = await _playlistRepository.GetWithSongsAsync(playlistId);
            if (playlist == null)
                throw new NotFoundException($"Playlist with ID {playlistId} not found");
                
            var song = await _songRepository.GetByIdAsync(songId);
            if (song == null)
                throw new NotFoundException($"Song with ID {songId} not found");
                
            // Check if song is already in playlist
            if (_playlistDomainService.HasSong(playlist, songId))
            {
                throw new BusinessRuleException("Song is already in the playlist");
            }
                
            // Check if playlist has reached maximum capacity
            if (!_playlistDomainService.CanAddSongToPlaylist(playlist, song))
            {
                throw new BusinessRuleException("Playlist has reached maximum capacity");
            }
            
            await _playlistRepository.AddSongAsync(playlistId, songId);
        }
        
        public async Task RemoveSongFromPlaylistAsync(int playlistId, int songId)
        {
            var playlist = await _playlistRepository.GetWithSongsAsync(playlistId);
            if (playlist == null)
                throw new NotFoundException($"Playlist with ID {playlistId} not found");
                
            // Check if song is in playlist
            if (!_playlistDomainService.HasSong(playlist, songId))
            {
                throw new BusinessRuleException("Song is not in the playlist");
            }
            
            await _playlistRepository.RemoveSongAsync(playlistId, songId);
        }
        
        public async Task<string> GetPlaylistStatisticsAsync(int id)
        {
            var playlist = await _playlistRepository.GetWithSongsAsync(id);
            if (playlist == null)
                return "Playlist not found";
                
            return _playlistDomainService.GetPlaylistStatistics(playlist);
        }
        
        public async Task<int> CalculateTotalDurationAsync(int id)
        {
            var playlist = await _playlistRepository.GetWithSongsAsync(id);
            if (playlist == null)
                return 0;
                
            return _playlistDomainService.CalculateTotalDuration(playlist);
        }
    }
}