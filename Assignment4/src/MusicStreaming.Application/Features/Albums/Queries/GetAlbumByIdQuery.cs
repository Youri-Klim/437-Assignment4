using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Queries
{
    public class GetAlbumByIdQuery : IRequest<AlbumDto>
    {
        public int Id { get; set; }
    }

    public class GetAlbumByIdQueryHandler : IRequestHandler<GetAlbumByIdQuery, AlbumDto>
    {
        private readonly IAlbumRepository _albumRepository;
        
        public GetAlbumByIdQueryHandler(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }
        
        public async Task<AlbumDto> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken)
        {
            return await _albumRepository.GetByIdAsync(request.Id);
        }
    }
}