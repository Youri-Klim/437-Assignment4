using FluentValidation;
using MediatR;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Commands
{
    public class AddSongToPlaylistCommand : IRequest<bool>
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
    }

    public class AddSongToPlaylistCommandValidator : AbstractValidator<AddSongToPlaylistCommand>
    {
        public AddSongToPlaylistCommandValidator()
        {
            RuleFor(x => x.PlaylistId)
                .GreaterThan(0).WithMessage("A valid playlist ID is required");
                
            RuleFor(x => x.SongId)
                .GreaterThan(0).WithMessage("A valid song ID is required");
        }
    }

    public class AddSongToPlaylistCommandHandler : IRequestHandler<AddSongToPlaylistCommand, bool>
    {
        private readonly IPlaylistRepository _playlistRepository;
        
        public AddSongToPlaylistCommandHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        
        public async Task<bool> Handle(AddSongToPlaylistCommand request, CancellationToken cancellationToken)
        {
            await _playlistRepository.AddSongAsync(request.PlaylistId, request.SongId);
            return true;
        }
    }
}