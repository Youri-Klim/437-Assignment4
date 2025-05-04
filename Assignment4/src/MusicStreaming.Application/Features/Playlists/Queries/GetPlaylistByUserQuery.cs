using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Queries
{
    public class GetPlaylistsByUserQuery : IRequest<IReadOnlyList<PlaylistDto>>
    {
        public string UserId { get; set; }
    }

    public class GetPlaylistsByUserQueryHandler : IRequestHandler<GetPlaylistsByUserQuery, IReadOnlyList<PlaylistDto>>
    {
        private readonly IPlaylistRepository _playlistRepository;
        
        public GetPlaylistsByUserQueryHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        
        public async Task<IReadOnlyList<PlaylistDto>> Handle(GetPlaylistsByUserQuery request, CancellationToken cancellationToken)
        {
            return await _playlistRepository.GetByUserIdAsync(request.UserId);
        }
    }
}