using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
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
        private readonly IArtistRepository _artistRepository;

        public GetArtistsQueryHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<IReadOnlyList<ArtistDto>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
        {
            return await _artistRepository.ListAllAsync();
        }
    }
}