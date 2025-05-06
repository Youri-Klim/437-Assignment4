using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Infrastructure.Data;
using MusicStreaming.Infrastructure.Repositories;

namespace MusicStreaming.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // Database context
            services.AddDbContext<MusicStreamingDbContext>(options =>
                options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(MusicStreamingDbContext).Assembly.FullName)));
            
            // Repositories
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}