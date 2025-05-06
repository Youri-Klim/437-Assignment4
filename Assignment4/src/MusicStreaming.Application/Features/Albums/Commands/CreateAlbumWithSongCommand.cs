using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Commands
{
    public class CreateAlbumWithSongCommand : IRequest<int>
    {
        // Album properties
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
        public required string Genre { get; set; }
        public int ArtistId { get; set; }
        
        // Song properties
        public required string SongTitle { get; set; }
        public int SongDurationInSeconds { get; set; }
        public required string SongGenre { get; set; }
        public DateTime SongReleaseDate { get; set; }
    }

    public class CreateAlbumWithSongCommandValidator : AbstractValidator<CreateAlbumWithSongCommand>
    {
        public CreateAlbumWithSongCommandValidator()
        {
            // Album validations
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Album title is required")
                .MaximumLength(100).WithMessage("Album title cannot exceed 100 characters");
                
            RuleFor(x => x.ReleaseYear)
                .GreaterThanOrEqualTo(1900).WithMessage("Release year cannot be earlier than 1900")
                .LessThanOrEqualTo(2100).WithMessage("Release year cannot be later than 2100");
                
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Album genre is required")
                .MaximumLength(50).WithMessage("Album genre cannot exceed 50 characters");
                
            RuleFor(x => x.ArtistId)
                .GreaterThan(0).WithMessage("A valid artist ID is required");
                
            // Song validations
            RuleFor(x => x.SongTitle)
                .NotEmpty().WithMessage("Song title is required")
                .MaximumLength(100).WithMessage("Song title cannot exceed 100 characters");
                
            RuleFor(x => x.SongDurationInSeconds)
                .GreaterThan(0).WithMessage("Song duration must be greater than 0 seconds")
                .LessThan(3600).WithMessage("Song duration cannot exceed 1 hour");
                
            RuleFor(x => x.SongGenre)
                .NotEmpty().WithMessage("Song genre is required")
                .MaximumLength(50).WithMessage("Song genre cannot exceed 50 characters");
        }
    }

    public class CreateAlbumWithSongCommandHandler : IRequestHandler<CreateAlbumWithSongCommand, int>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly ISongRepository _songRepository;
        
        public CreateAlbumWithSongCommandHandler(IAlbumRepository albumRepository, ISongRepository songRepository)
        {
            _albumRepository = albumRepository;
            _songRepository = songRepository;
        }
        
        public async Task<int> Handle(CreateAlbumWithSongCommand request, CancellationToken cancellationToken)
        {
            // Create album
            var albumDto = new CreateAlbumDto
            {
                Title = request.Title,
                ReleaseYear = request.ReleaseYear,
                Genre = request.Genre,
                ArtistId = request.ArtistId
            };
            
            var albumId = await _albumRepository.AddAsync(albumDto);
            
            if (albumId > 0)
            {
                // Create song associated with the album
                var songDto = new CreateSongDto
                {
                    Title = request.SongTitle,
                    DurationInSeconds = request.SongDurationInSeconds,
                    ReleaseDate = request.SongReleaseDate,
                    Genre = request.SongGenre,
                    AlbumId = albumId
                };
                
                await _songRepository.AddAsync(songDto);
            }
            
            return albumId;
        }
    }
}