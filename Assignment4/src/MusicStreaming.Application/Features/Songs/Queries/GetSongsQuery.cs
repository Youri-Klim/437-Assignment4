using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Queries
{
    public class GetSongsQuery : IRequest<IReadOnlyList<SongDto>>
    {
        // No parameters needed as we're retrieving all songs
    }

    public class GetSongsQueryHandler : IRequestHandler<GetSongsQuery, IReadOnlyList<SongDto>>
    {
        private readonly ISongRepository _songRepository;
        
        public GetSongsQueryHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }
        
        public async Task<IReadOnlyList<SongDto>> Handle(GetSongsQuery request, CancellationToken cancellationToken)
        {
            return await _songRepository.ListAllAsync();
        }
    }
}