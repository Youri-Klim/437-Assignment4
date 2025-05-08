using FluentValidation;
using MediatR;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Commands
{
    public class DeletePlaylistCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeletePlaylistCommandValidator : AbstractValidator<DeletePlaylistCommand>
    {
        public DeletePlaylistCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid playlist ID is required");
        }
    }

    public class DeletePlaylistCommandHandler : IRequestHandler<DeletePlaylistCommand, bool>
    {
        private readonly PlaylistService _playlistService;
        
        public DeletePlaylistCommandHandler(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        
        public async Task<bool> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _playlistService.DeleteAsync(request.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}