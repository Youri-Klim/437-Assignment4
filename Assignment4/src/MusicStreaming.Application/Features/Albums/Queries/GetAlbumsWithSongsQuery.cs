using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Queries
{
    public class GetAlbumWithSongsQuery : IRequest<AlbumDto?>
    {
        public int Id { get; set; }
    }

    public class GetAlbumWithSongsQueryHandler : IRequestHandler<GetAlbumWithSongsQuery, AlbumDto?>
    {
        private readonly AlbumService _albumService;
        
        public GetAlbumWithSongsQueryHandler(AlbumService albumService)
        {
            _albumService = albumService;
        }
        
        public async Task<AlbumDto?> Handle(GetAlbumWithSongsQuery request, CancellationToken cancellationToken)
        {
            // Use the service method that fetches album with songs
            return await _albumService.GetWithSongsAsync(request.Id);
        }
    }
}