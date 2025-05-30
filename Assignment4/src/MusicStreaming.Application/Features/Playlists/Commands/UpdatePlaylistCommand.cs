using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Commands
{
    public class UpdatePlaylistCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
    }

    public class UpdatePlaylistCommandValidator : AbstractValidator<UpdatePlaylistCommand>
    {
        public UpdatePlaylistCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid playlist ID is required");
                
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
        }
    }

    public class UpdatePlaylistCommandHandler : IRequestHandler<UpdatePlaylistCommand, bool>
    {
        private readonly PlaylistService _playlistService;
        
        public UpdatePlaylistCommandHandler(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        
        public async Task<bool> Handle(UpdatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlistDto = new UpdatePlaylistDto
            {
                Id = request.Id,
                Title = request.Title
            };
            
            await _playlistService.UpdateAsync(playlistDto);
            return true;
        }
    }
}