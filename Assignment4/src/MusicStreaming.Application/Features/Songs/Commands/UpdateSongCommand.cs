using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Commands
{
    public class UpdateSongCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public required string Genre { get; set; }
        public int AlbumId { get; set; }
    }

    public class UpdateSongCommandValidator : AbstractValidator<UpdateSongCommand>
    {
        public UpdateSongCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid song ID is required");
                
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

    public class UpdateSongCommandHandler : IRequestHandler<UpdateSongCommand, bool>
    {
        private readonly SongService _songService;
        
        public UpdateSongCommandHandler(SongService songService)
        {
            _songService = songService;
        }
        
        public async Task<bool> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var songDto = new UpdateSongDto
                {
                    Id = request.Id,
                    Title = request.Title,
                    Duration = request.Duration,
                    ReleaseDate = request.ReleaseDate,
                    Genre = request.Genre,
                    AlbumId = request.AlbumId
                };
                
                await _songService.UpdateAsync(songDto);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}