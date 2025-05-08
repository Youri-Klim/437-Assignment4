using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Queries
{
    public class GetAlbumsQuery : IRequest<IReadOnlyList<AlbumDto>>
    {
    }

    public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, IReadOnlyList<AlbumDto>>
    {
        private readonly AlbumService _albumService;
        
        public GetAlbumsQueryHandler(AlbumService albumService)
        {
            _albumService = albumService;
        }
        
        public async Task<IReadOnlyList<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            return await _albumService.ListAllAsync();
        }
    }
}