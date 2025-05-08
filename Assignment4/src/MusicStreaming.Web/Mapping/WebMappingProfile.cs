using AutoMapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Features.Songs.Commands;
using MusicStreaming.Application.Features.Albums.Commands;
using MusicStreaming.Application.Features.Artists.Commands;
using MusicStreaming.Application.Features.Playlists.Commands;
using MusicStreaming.Web.ViewModels;
using MusicStreaming.Core.Entities;

namespace MusicStreaming.Web.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            // Song mappings
            CreateMap<SongDto, SongViewModel>();
            CreateMap<SongDto, SongDetailViewModel>();
            CreateMap<SongDto, EditSongViewModel>();
            CreateMap<SongViewModel, SongDetailViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.AlbumTitle, opt => opt.MapFrom(src => src.AlbumTitle))
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.ArtistName));
            CreateMap<CreateSongViewModel, CreateSongCommand>();
            CreateMap<EditSongViewModel, UpdateSongCommand>();
            
            // Album mappings
            CreateMap<AlbumDto, AlbumViewModel>();
            CreateMap<AlbumDto, AlbumDetailViewModel>();
            CreateMap<AlbumDto, EditAlbumViewModel>();
            CreateMap<CreateAlbumViewModel, CreateAlbumCommand>();
            CreateMap<EditAlbumViewModel, UpdateAlbumCommand>();
            
            // Album with Songs mappings
            CreateMap<CreateAlbumWithSongViewModel, CreateAlbumWithSongCommand>();
            
            // Artist mappings
            CreateMap<ArtistDto, ArtistViewModel>();
            CreateMap<ArtistDto, ArtistDetailViewModel>()
                .ForMember(dest => dest.Albums, opt => opt.MapFrom(src => src.Albums));
            CreateMap<ArtistDto, EditArtistViewModel>();
            CreateMap<CreateArtistViewModel, CreateArtistCommand>();
            CreateMap<EditArtistViewModel, UpdateArtistCommand>();
            
            // Playlist mappings
            CreateMap<PlaylistDto, PlaylistViewModel>();
            CreateMap<PlaylistDto, PlaylistDetailViewModel>()
                .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => src.Songs));
            CreateMap<PlaylistDto, EditPlaylistViewModel>();
            CreateMap<EditPlaylistViewModel, UpdatePlaylistCommand>();
            CreateMap<CreatePlaylistViewModel, CreatePlaylistCommand>();
            CreateMap<Playlist, PlaylistDto>()
    .ForMember(dest => dest.Songs, opt => opt.MapFrom(src => 
        src.PlaylistSongs.Select(ps => ps.Song)));
            
            // User mappings
            CreateMap<UserDto, UserViewModel>();
        }
    }
}