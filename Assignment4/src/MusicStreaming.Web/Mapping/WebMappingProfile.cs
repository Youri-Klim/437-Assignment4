using AutoMapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Features.Songs.Commands;
using MusicStreaming.Application.Features.Albums.Commands;
using MusicStreaming.Application.Features.Artists.Commands;
using MusicStreaming.Application.Features.Playlists.Commands;
using MusicStreaming.Web.ViewModels;

namespace MusicStreaming.Web.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            // Song mappings
            CreateMap<SongDto, SongViewModel>();
            CreateMap<SongDto, SongDetailViewModel>();
            CreateMap<SongDto, EditSongViewModel>();  // Added for Edit view
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
            CreateMap<AlbumDto, EditAlbumViewModel>(); // Added for Edit view
            CreateMap<CreateAlbumViewModel, CreateAlbumCommand>();
            CreateMap<EditAlbumViewModel, UpdateAlbumCommand>();
            
            // Album with Songs mappings
            CreateMap<CreateAlbumWithSongViewModel, CreateAlbumWithSongCommand>(); // Added for Create Album with Song
            
            // Artist mappings
            CreateMap<ArtistDto, ArtistViewModel>();
            CreateMap<ArtistDto, ArtistDetailViewModel>(); // Added for Details view
            CreateMap<ArtistDto, EditArtistViewModel>(); // Added for Edit view
            CreateMap<CreateArtistViewModel, CreateArtistCommand>();
            CreateMap<EditArtistViewModel, UpdateArtistCommand>();
            
            // Playlist mappings
            CreateMap<PlaylistDto, PlaylistViewModel>();
            
            // User mappings
            CreateMap<UserDto, UserViewModel>();
        }
    }
}