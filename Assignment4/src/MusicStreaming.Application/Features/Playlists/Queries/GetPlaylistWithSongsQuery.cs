using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Queries
{
    public class GetPlaylistWithSongsQuery : IRequest<PlaylistDto?>
    {
        public int Id { get; set; }
    }

    public class GetPlaylistWithSongsQueryHandler : IRequestHandler<GetPlaylistWithSongsQuery, PlaylistDto?>
    {
        private readonly PlaylistService _playlistService;
        
        public GetPlaylistWithSongsQueryHandler(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        
        public async Task<PlaylistDto?> Handle(GetPlaylistWithSongsQuery request, CancellationToken cancellationToken)
        {
            return await _playlistService.GetWithSongsAsync(request.Id);
        }
    }
}