using AutoMapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Core.Entities;

namespace MusicStreaming.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Song mappings
            CreateMap<MusicStreaming.Core.Entities.Song, SongDto>()
                .ForMember(d => d.AlbumTitle, o => o.MapFrom(s => s.Album != null ? s.Album.Title : null))
                .ForMember(d => d.ArtistName, o => o.MapFrom(s => s.Album != null ? s.Album.Artist.Name : null));
            
            // Album mappings
            CreateMap<Album, AlbumDto>()
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist != null ? src.Artist.Name : string.Empty));
                
            CreateMap<UpdateAlbumDto, Album>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.ArtistId, opt => opt.MapFrom(src => src.ArtistId));
                
            CreateMap<CreateAlbumDto, Album>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ReleaseYear, opt => opt.MapFrom(src => src.ReleaseYear))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
                .ForMember(dest => dest.ArtistId, opt => opt.MapFrom(src => src.ArtistId));
                
            // Artist mappings
            CreateMap<MusicStreaming.Core.Entities.Artist, ArtistDto>();
           CreateMap<Artist, ArtistDto>()
                .ForMember(dest => dest.Albums, opt => opt.MapFrom(src => src.Albums));
            
            
            // Playlist mappings
            CreateMap<MusicStreaming.Core.Entities.Playlist, PlaylistDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
                
            // User mappings
            CreateMap<MusicStreaming.Core.Entities.User, UserDto>();
        }
    }
}