using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Queries
{
    public class GetSongsByAlbumQuery : IRequest<IReadOnlyList<SongDto>>
    {
        public int AlbumId { get; set; }
    }

    public class GetSongsByAlbumQueryHandler : IRequestHandler<GetSongsByAlbumQuery, IReadOnlyList<SongDto>>
    {
        private readonly SongService _songService;
        
        public GetSongsByAlbumQueryHandler(SongService songService)
        {
            _songService = songService;
        }
        
        public async Task<IReadOnlyList<SongDto>> Handle(GetSongsByAlbumQuery request, CancellationToken cancellationToken)
        {
            return await _songService.GetByAlbumIdAsync(request.AlbumId);
        }
    }
}