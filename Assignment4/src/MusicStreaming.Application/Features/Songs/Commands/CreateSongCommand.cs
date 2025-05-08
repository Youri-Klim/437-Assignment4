using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Commands
{
    public class CreateSongCommand : IRequest<int>
    {
        public required string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Genre { get; set; }
        public int AlbumId { get; set; }
    }

    public class CreateSongCommandValidator : AbstractValidator<CreateSongCommand>
    {
        public CreateSongCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
                
            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 seconds")
                .LessThan(3 * 60 * 60).WithMessage("Duration cannot exceed 3 hours");
                
            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Release date cannot be in the future");
                
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters");
                
            RuleFor(x => x.AlbumId)
                .GreaterThan(0).WithMessage("A valid album ID is required");
        }
    }

    public class CreateSongCommandHandler : IRequestHandler<CreateSongCommand, int>
    {
        private readonly SongService _songService;
        
        public CreateSongCommandHandler(SongService songService)
        {
            _songService = songService;
        }
        
        public async Task<int> Handle(CreateSongCommand request, CancellationToken cancellationToken)
        {
            var songDto = new CreateSongDto
            {
                Title = request.Title,
                Duration = request.Duration,
                ReleaseDate = request.ReleaseDate,
                Genre = request.Genre,
                AlbumId = request.AlbumId
            };
                
            return await _songService.AddAsync(songDto);
        }
    }
}