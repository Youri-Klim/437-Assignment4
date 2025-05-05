using AutoMapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Features.Songs.Commands;
using MusicStreaming.Application.Features.Albums.Commands;
using MusicStreaming.Application.Features.Artists.Commands;
using MusicStreaming.Application.Features.Playlists.Commands;
using MusicStreaming.Web.Models;

namespace MusicStreaming.Web.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            // Song mappings
            CreateMap<SongDto, SongViewModel>();
            CreateMap<CreateSongViewModel, CreateSongCommand>();
            CreateMap<EditSongViewModel, UpdateSongCommand>();

            // Album mappings
            CreateMap<AlbumDto, AlbumViewModel>();
            CreateMap<AlbumDto, AlbumDetailViewModel>();
            CreateMap<CreateAlbumViewModel, CreateAlbumCommand>();
            CreateMap<EditAlbumViewModel, UpdateAlbumCommand>();

            // Artist mappings
            CreateMap<ArtistDto, ArtistViewModel>();
            CreateMap<CreateArtistViewModel, CreateArtistCommand>();
            CreateMap<EditArtistViewModel, UpdateArtistCommand>();

            // Playlist mappings
            CreateMap<PlaylistDto, PlaylistViewModel>();
            CreateMap<PlaylistDto, EditPlaylistViewModel>();
            CreateMap<CreatePlaylistViewModel, CreatePlaylistCommand>();
        }
    }
}