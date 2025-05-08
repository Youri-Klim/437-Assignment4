using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Artists.Queries
{
    public class GetArtistByIdQuery : IRequest<ArtistDto>
    {
        public int Id { get; set; }
    }

    public class GetArtistByIdQueryHandler : IRequestHandler<GetArtistByIdQuery, ArtistDto?>
    {
        private readonly ArtistService _artistService;
        
        public GetArtistByIdQueryHandler(ArtistService artistService)
        {
            _artistService = artistService;
        }
        
        public async Task<ArtistDto?> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
        {
            return await _artistService.GetWithAlbumsAsync(request.Id);
        }
    }
}