using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Queries
{
    public class GetPlaylistByIdQuery : IRequest<PlaylistDto?>
    {
        public int Id { get; set; }
    }

    public class GetPlaylistByIdQueryHandler : IRequestHandler<GetPlaylistByIdQuery, PlaylistDto?>
    {
        private readonly PlaylistService _playlistService;
        
        public GetPlaylistByIdQueryHandler(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        
        public async Task<PlaylistDto?> Handle(GetPlaylistByIdQuery request, CancellationToken cancellationToken)
        {
            return await _playlistService.GetByIdAsync(request.Id);
        }
    }
}