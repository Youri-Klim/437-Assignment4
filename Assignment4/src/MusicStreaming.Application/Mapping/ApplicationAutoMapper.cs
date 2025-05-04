using AutoMapper;
using MusicStreaming.Application.DTOs;

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
            CreateMap<MusicStreaming.Core.Entities.Album, AlbumDto>()
                .ForMember(d => d.ArtistName, o => o.MapFrom(s => s.Artist != null ? s.Artist.Name : null));
                
            // Artist mappings
            CreateMap<MusicStreaming.Core.Entities.Artist, ArtistDto>();
            
            // Playlist mappings
            CreateMap<MusicStreaming.Core.Entities.Playlist, PlaylistDto>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
                
            // User mappings (assuming you have a User entity)
            CreateMap<MusicStreaming.Core.Entities.User, UserDto>();
        }
    }
}