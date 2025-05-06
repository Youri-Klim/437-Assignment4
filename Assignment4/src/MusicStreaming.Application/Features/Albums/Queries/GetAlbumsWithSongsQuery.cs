using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Queries
{
    public class GetAlbumWithSongsQuery : IRequest<AlbumDto?>
    {
        public int Id { get; set; }
    }

    public class GetAlbumWithSongsQueryHandler : IRequestHandler<GetAlbumWithSongsQuery, AlbumDto?>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly ISongRepository _songRepository;
        
        public GetAlbumWithSongsQueryHandler(IAlbumRepository albumRepository, ISongRepository songRepository)
        {
            _albumRepository = albumRepository;
            _songRepository = songRepository;
        }
        
        public async Task<AlbumDto?> Handle(GetAlbumWithSongsQuery request, CancellationToken cancellationToken)
        {
            var album = await _albumRepository.GetByIdAsync(request.Id);
            if (album != null)
            {
                var songs = await _songRepository.GetByAlbumIdAsync(request.Id);
                album.Songs = songs.ToList(); // Convert IReadOnlyList to List
            }
            return album;
        }
    }
}