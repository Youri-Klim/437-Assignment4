using MediatR;
using MusicStreaming.Application.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;
using MusicStreaming.Application.Interfaces.Repositories;

namespace MusicStreaming.Application.Features.Playlists.Queries
{
    public class GetPlaylistWithSongsQuery : IRequest<PlaylistDto?>
    {
        public int Id { get; set; }
    }

    public class GetPlaylistWithSongsQueryHandler : IRequestHandler<GetPlaylistWithSongsQuery, PlaylistDto?>
    {
        private readonly IPlaylistRepository _playlistRepository;

        public GetPlaylistWithSongsQueryHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }

        public async Task<PlaylistDto?> Handle(GetPlaylistWithSongsQuery request, CancellationToken cancellationToken)
        {
            return await _playlistRepository.GetWithSongsAsync(request.Id);
        }
    }
}