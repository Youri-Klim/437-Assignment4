using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Queries
{
    public class GetAlbumByIdQuery : IRequest<AlbumDto>
    {
        public int Id { get; set; }
    }

    public class GetAlbumByIdQueryHandler : IRequestHandler<GetAlbumByIdQuery, AlbumDto?>
    {
        private readonly AlbumService _albumService;
        
        public GetAlbumByIdQueryHandler(AlbumService albumService)
        {
            _albumService = albumService;
        }
        
        public async Task<AlbumDto?> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken)
        {
            return await _albumService.GetByIdAsync(request.Id);
        }
    }
}