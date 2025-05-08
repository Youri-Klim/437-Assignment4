using FluentValidation;
using MediatR;
using MusicStreaming.Application.Services;
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
        private readonly PlaylistService _playlistService;
        
        public AddSongToPlaylistCommandHandler(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        
        public async Task<bool> Handle(AddSongToPlaylistCommand request, CancellationToken cancellationToken)
        {
            await _playlistService.AddSongToPlaylistAsync(request.PlaylistId, request.SongId);
            return true;
        }
    }
}