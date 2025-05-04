using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
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
        private readonly IAlbumRepository _albumRepository;
        
        public GetAlbumsQueryHandler(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }
        
        public async Task<IReadOnlyList<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            return await _albumRepository.ListAllAsync();
        }
    }
}