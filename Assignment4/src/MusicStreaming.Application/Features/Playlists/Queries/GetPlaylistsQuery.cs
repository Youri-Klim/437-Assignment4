using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
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
        private readonly IPlaylistRepository _playlistRepository;
        
        public GetPlaylistsQueryHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        
        public async Task<IReadOnlyList<PlaylistDto>> Handle(GetPlaylistsQuery request, CancellationToken cancellationToken)
        {
            return await _playlistRepository.ListAllAsync();
        }
    }
}