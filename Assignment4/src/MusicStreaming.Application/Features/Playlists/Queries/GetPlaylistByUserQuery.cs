using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Queries
{
    public class GetPlaylistsByUserQuery : IRequest<IReadOnlyList<PlaylistDto>>
    {
        public required string UserId { get; set; }
    }

    public class GetPlaylistsByUserQueryHandler : IRequestHandler<GetPlaylistsByUserQuery, IReadOnlyList<PlaylistDto>>
    {
        private readonly PlaylistService _playlistService;
        
        public GetPlaylistsByUserQueryHandler(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        
        public async Task<IReadOnlyList<PlaylistDto>> Handle(GetPlaylistsByUserQuery request, CancellationToken cancellationToken)
        {
            return await _playlistService.GetByUserIdAsync(request.UserId);
        }
    }
}