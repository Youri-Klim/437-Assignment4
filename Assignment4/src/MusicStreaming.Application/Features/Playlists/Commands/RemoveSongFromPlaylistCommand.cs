using FluentValidation;
using MediatR;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Commands
{
    public class RemoveSongFromPlaylistCommand : IRequest<bool>
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
    }

    public class RemoveSongFromPlaylistCommandValidator : AbstractValidator<RemoveSongFromPlaylistCommand>
    {
        public RemoveSongFromPlaylistCommandValidator()
        {
            RuleFor(x => x.PlaylistId)
                .GreaterThan(0).WithMessage("A valid playlist ID is required");
                
            RuleFor(x => x.SongId)
                .GreaterThan(0).WithMessage("A valid song ID is required");
        }
    }

    public class RemoveSongFromPlaylistCommandHandler : IRequestHandler<RemoveSongFromPlaylistCommand, bool>
    {
        private readonly IPlaylistRepository _playlistRepository;
        
        public RemoveSongFromPlaylistCommandHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        
        public async Task<bool> Handle(RemoveSongFromPlaylistCommand request, CancellationToken cancellationToken)
        {
            await _playlistRepository.RemoveSongAsync(request.PlaylistId, request.SongId);
            return true;
        }
    }
}