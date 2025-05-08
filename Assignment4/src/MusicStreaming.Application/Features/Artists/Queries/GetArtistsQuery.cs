using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Artists.Queries
{
    public class GetArtistsQuery : IRequest<IReadOnlyList<ArtistDto>>
    {
        // No parameters needed for getting all artists
    }

    public class GetArtistsQueryHandler : IRequestHandler<GetArtistsQuery, IReadOnlyList<ArtistDto>>
    {
        private readonly ArtistService _artistService;

        public GetArtistsQueryHandler(ArtistService artistService)
        {
            _artistService = artistService;
        }

        public async Task<IReadOnlyList<ArtistDto>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
        {
            return await _artistService.ListAllAsync();
        }
    }
}