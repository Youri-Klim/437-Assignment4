using Microsoft.EntityFrameworkCore;
using MusicStreaming.Core.Entities;

namespace MusicStreaming.Infrastructure.Data
{
    public class MusicStreamingDbContext : DbContext
    {
        public MusicStreamingDbContext(DbContextOptions<MusicStreamingDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships and constraints
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicStreamingDbContext).Assembly);
            
            // Configure many-to-many relationship for PlaylistSong
            modelBuilder.Entity<PlaylistSong>()
                .HasKey(ps => new { ps.PlaylistId, ps.SongId });
                
            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(ps => ps.PlaylistId);
                
            modelBuilder.Entity<PlaylistSong>()
                .HasOne(ps => ps.Song)
                .WithMany()
                .HasForeignKey(ps => ps.SongId);
        }
    }
}