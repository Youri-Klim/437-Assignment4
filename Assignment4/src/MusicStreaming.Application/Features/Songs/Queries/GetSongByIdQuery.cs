using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Queries
{
    public class GetSongByIdQuery : IRequest<SongDto>
    {
        public int Id { get; set; }
    }

    public class GetSongByIdQueryHandler : IRequestHandler<GetSongByIdQuery, SongDto?>
    {
        private readonly SongService _songService;
        
        public GetSongByIdQueryHandler(SongService songService)
        {
            _songService = songService;
        }
        
        public async Task<SongDto?> Handle(GetSongByIdQuery request, CancellationToken cancellationToken)
        {
            return await _songService.GetByIdAsync(request.Id);
        }
    }
}