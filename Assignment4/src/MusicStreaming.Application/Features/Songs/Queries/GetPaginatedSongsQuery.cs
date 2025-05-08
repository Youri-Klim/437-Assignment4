using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Queries
{
    public class GetPaginatedSongsQuery : IRequest<(List<SongDto> Songs, int TotalCount)>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetPaginatedSongsQueryHandler : IRequestHandler<GetPaginatedSongsQuery, (List<SongDto> Songs, int TotalCount)>
    {
        private readonly SongService _songService;

        public GetPaginatedSongsQueryHandler(SongService songService)
        {
            _songService = songService;
        }

        public async Task<(List<SongDto> Songs, int TotalCount)> Handle(GetPaginatedSongsQuery request, CancellationToken cancellationToken)
        {
            return await _songService.GetPaginatedAsync(request.PageNumber, request.PageSize);
        }
    }
}