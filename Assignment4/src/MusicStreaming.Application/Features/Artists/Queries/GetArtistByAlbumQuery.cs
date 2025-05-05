using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
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
        private readonly IArtistRepository _artistRepository;
        
        public GetArtistsWithAlbumsQueryHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }
        
        public async Task<IReadOnlyList<ArtistDto>> Handle(GetArtistsWithAlbumsQuery request, CancellationToken cancellationToken)
        {
            return await _artistRepository.GetArtistsWithAlbumsAsync();
        }
    }
}