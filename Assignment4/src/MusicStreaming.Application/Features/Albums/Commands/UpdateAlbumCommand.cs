using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Commands
{
    public class UpdateAlbumCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
        public required string Genre { get; set; }
        public int ArtistId { get; set; }
    }

    public class UpdateAlbumCommandValidator : AbstractValidator<UpdateAlbumCommand>
    {
        public UpdateAlbumCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid album ID is required");
                
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
                
            RuleFor(x => x.ReleaseYear)
                .NotEmpty().WithMessage("Release year is required")
                .GreaterThan(1900).WithMessage("Release year must be after 1900")
                .LessThanOrEqualTo(System.DateTime.Now.Year).WithMessage("Release year cannot be in the future");
                
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters");
                
            RuleFor(x => x.ArtistId)
                .GreaterThan(0).WithMessage("A valid artist ID is required");
        }
    }

    public class UpdateAlbumCommandHandler : IRequestHandler<UpdateAlbumCommand, bool>
    {
        private readonly IAlbumRepository _albumRepository;
        
        public UpdateAlbumCommandHandler(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }
        
        public async Task<bool> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
        {
            var albumDto = new UpdateAlbumDto
            {
                Id = request.Id,
                Title = request.Title,
                ReleaseYear = request.ReleaseYear,
                Genre = request.Genre,
                ArtistId = request.ArtistId
            };
            
            await _albumRepository.UpdateAsync(albumDto);
            return true;
        }
    }
}