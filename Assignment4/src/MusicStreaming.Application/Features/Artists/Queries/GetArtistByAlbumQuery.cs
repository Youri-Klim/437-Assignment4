using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Artists.Queries
{
    public class GetArtistsWithAlbumsQuery : IRequest<IReadOnlyList<ArtistDto>>
    {
        // No parameters needed as we're retrieving all artists with their albums
    }

    public class GetArtistsWithAlbumsQueryHandler : IRequestHandler<GetArtistsWithAlbumsQuery, IReadOnlyList<ArtistDto>>
    {
        private readonly ArtistService _artistService;
        
        public GetArtistsWithAlbumsQueryHandler(ArtistService artistService)
        {
            _artistService = artistService;
        }
        
        public async Task<IReadOnlyList<ArtistDto>> Handle(GetArtistsWithAlbumsQuery request, CancellationToken cancellationToken)
        {
    // Actually get artists WITH their albums
    return await _artistService.GetArtistsWithAlbumsAsync();
        }
    }
}