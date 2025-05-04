using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Queries
{
    public class GetPlaylistByIdQuery : IRequest<PlaylistDto>
    {
        public int Id { get; set; }
    }

    public class GetPlaylistByIdQueryHandler : IRequestHandler<GetPlaylistByIdQuery, PlaylistDto>
    {
        private readonly IPlaylistRepository _playlistRepository;
        
        public GetPlaylistByIdQueryHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        
        public async Task<PlaylistDto> Handle(GetPlaylistByIdQuery request, CancellationToken cancellationToken)
        {
            return await _playlistRepository.GetByIdAsync(request.Id);
        }
    }
}