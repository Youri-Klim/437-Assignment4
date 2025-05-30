using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Commands
{
    public class CreateAlbumCommand : IRequest<int>
    {
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
        public required string Genre { get; set; }
        public int ArtistId { get; set; }
    }

    public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
    {
        public CreateAlbumCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
                
            RuleFor(x => x.ReleaseYear)
                .GreaterThanOrEqualTo(1900).WithMessage("Release year cannot be earlier than 1900")
                .LessThanOrEqualTo(2100).WithMessage("Release year cannot be later than 2100");
                
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters");
                
            RuleFor(x => x.ArtistId)
                .GreaterThan(0).WithMessage("A valid artist ID is required");
        }
    }

    public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, int>
    {
        private readonly AlbumService _albumService;
        
        public CreateAlbumCommandHandler(AlbumService albumService)
        {
            _albumService = albumService;
        }
        
        public async Task<int> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            var albumDto = new CreateAlbumDto
            {
                Title = request.Title,
                ReleaseYear = request.ReleaseYear,
                Genre = request.Genre,
                ArtistId = request.ArtistId
            };
            
            return await _albumService.AddAsync(albumDto);
        }
    }
}