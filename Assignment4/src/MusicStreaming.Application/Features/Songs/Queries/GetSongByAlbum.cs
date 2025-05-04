using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
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
        private readonly ISongRepository _songRepository;
        
        public GetSongsByAlbumQueryHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }
        
        public async Task<IReadOnlyList<SongDto>> Handle(GetSongsByAlbumQuery request, CancellationToken cancellationToken)
        {
            return await _songRepository.GetByAlbumIdAsync(request.AlbumId);
        }
    }
}