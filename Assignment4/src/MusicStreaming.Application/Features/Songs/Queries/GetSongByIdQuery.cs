using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Queries
{
    public class GetSongByIdQuery : IRequest<SongDto>
    {
        public int Id { get; set; }
    }

    public class GetSongByIdQueryHandler : IRequestHandler<GetSongByIdQuery, SongDto>
    {
        private readonly ISongRepository _songRepository;
        
        public GetSongByIdQueryHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }
        
        public async Task<SongDto> Handle(GetSongByIdQuery request, CancellationToken cancellationToken)
        {
            return await _songRepository.GetByIdAsync(request.Id);
        }
    }
}