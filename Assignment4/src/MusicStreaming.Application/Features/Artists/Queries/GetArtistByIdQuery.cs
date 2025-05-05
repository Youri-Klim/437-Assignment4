using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Artists.Queries
{
    public class GetArtistByIdQuery : IRequest<ArtistDto>
    {
        public int Id { get; set; }
    }

    public class GetArtistByIdQueryHandler : IRequestHandler<GetArtistByIdQuery, ArtistDto>
    {
        private readonly IArtistRepository _artistRepository;
        
        public GetArtistByIdQueryHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }
        
        public async Task<ArtistDto> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
        {
            return await _artistRepository.GetByIdAsync(request.Id);
        }
    }
}