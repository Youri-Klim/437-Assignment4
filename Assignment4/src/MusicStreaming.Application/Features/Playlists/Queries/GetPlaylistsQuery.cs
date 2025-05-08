using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Queries
{
    public class GetPlaylistsQuery : IRequest<IReadOnlyList<PlaylistDto>>
    {
        // No parameters needed as we're retrieving all playlists
    }

    public class GetPlaylistsQueryHandler : IRequestHandler<GetPlaylistsQuery, IReadOnlyList<PlaylistDto>>
    {
        private readonly PlaylistService _playlistService;
        
        public GetPlaylistsQueryHandler(PlaylistService playlistService)
        {
            _playlistService = playlistService;
        }
        
        public async Task<IReadOnlyList<PlaylistDto>> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
        {
            return await _playlistService.ListAllAsync();
        }
    }
}