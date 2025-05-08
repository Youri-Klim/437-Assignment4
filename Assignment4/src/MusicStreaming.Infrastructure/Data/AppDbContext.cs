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

    // Seed data
    modelBuilder.Entity<Artist>().HasData(
        new Artist { Id = 1, Name = "Metallica", Genre = "Metal" },
        new Artist { Id = 2, Name = "Britney Spears", Genre = "Pop" },
        new Artist { Id = 3, Name = "Aphex Twin", Genre = "Electronic" },
        new Artist { Id = 4, Name = "SHINee", Genre = "K-pop" },
        new Artist { Id = 5, Name = "De La Soul", Genre = "Hip-hop" }
    );

    modelBuilder.Entity<Album>().HasData(
        new Album { Id = 1, Title = "And Justice for All", ReleaseYear = 1988, Genre = "Metal", ArtistId = 1 },
        new Album { Id = 2, Title = "Baby One More Time", ReleaseYear = 1999, Genre = "Pop", ArtistId = 2 },
        new Album { Id = 3, Title = "I Care Because You Do", ReleaseYear = 1995, Genre = "Electronic", ArtistId = 3 },
        new Album { Id = 4, Title = "1 of 1", ReleaseYear = 2016, Genre = "K-pop", ArtistId = 4 },
        new Album { Id = 5, Title = "3 Feet High and Rising", ReleaseYear = 1989, Genre = "Hip-hop", ArtistId = 5 },
        new Album { Id = 6, Title = "Reloaded", ReleaseYear = 2020, Genre = "Hip-hop", ArtistId = 5 },
        new Album { Id = 7, Title = "Enter the Metal", ReleaseYear = 1991, Genre = "Metal", ArtistId = 1 }
    );

    modelBuilder.Entity<Song>().HasData(
        new Song { Id = 1, Title = "One", Duration = 500, ReleaseDate = new DateTime(1988, 9, 7), Genre = "Metal", AlbumId = 1 },
        new Song { Id = 2, Title = "Enter Sandman", Duration = 330, ReleaseDate = new DateTime(1988, 7, 29), Genre = "Metal", AlbumId = 1 },
        new Song { Id = 3, Title = "Baby One More Time", Duration = 270, ReleaseDate = new DateTime(1999, 9, 28), Genre = "Pop", AlbumId = 2 },
        new Song { Id = 4, Title = "You Drive Me Crazy", Duration = 300, ReleaseDate = new DateTime(1999, 10, 5), Genre = "Pop", AlbumId = 2 },
        new Song { Id = 5, Title = "Windowlicker", Duration = 380, ReleaseDate = new DateTime(1995, 4, 10), Genre = "Electronic", AlbumId = 3 },
        new Song { Id = 6, Title = "Ventolin", Duration = 200, ReleaseDate = new DateTime(1995, 5, 15), Genre = "Electronic", AlbumId = 3 },
        new Song { Id = 7, Title = "View from the Top", Duration = 340, ReleaseDate = new DateTime(2016, 10, 14), Genre = "K-pop", AlbumId = 4 },
        new Song { Id = 8, Title = "Tell Me What to Do", Duration = 250, ReleaseDate = new DateTime(2016, 10, 14), Genre = "K-pop", AlbumId = 4 },
        new Song { Id = 9, Title = "Potholes in My Lawn", Duration = 230, ReleaseDate = new DateTime(1989, 3, 3), Genre = "Hip-hop", AlbumId = 5 },
        new Song { Id = 10, Title = "Me Myself and I", Duration = 210, ReleaseDate = new DateTime(1989, 3, 3), Genre = "Hip-hop", AlbumId = 5 },
        new Song { Id = 11, Title = "Push the Tempo", Duration = 220, ReleaseDate = new DateTime(2020, 7, 18), Genre = "Hip-hop", AlbumId = 6 },
        new Song { Id = 12, Title = "We Are The Champions", Duration = 350, ReleaseDate = new DateTime(1991, 1, 12), Genre = "Metal", AlbumId = 7 },
        new Song { Id = 13, Title = "Fade To Black", Duration = 400, ReleaseDate = new DateTime(1991, 3, 2), Genre = "Metal", AlbumId = 7 }
    );

    modelBuilder.Entity<User>().HasData(
        new User
        {
            Id = "1",
            Username = "Admin",
            Email = "mail@example.com",
            Password = "password",
            DateOfBirth = new DateTime(1990, 5, 15)
        },
        new User
        {
            Id = "2",
            Username = "maya_k",
            Email = "maya@example.com",
            Password = "password12",
            DateOfBirth = new DateTime(1992, 3, 22)
        },
        new User
        {
            Id = "3",
            Username = "omar_h",
            Email = "omar@example.com",
            Password = "password13",
            DateOfBirth = new DateTime(1988, 7, 10)
        },
        new User
        {
            Id = "4",
            Username = "layla_az",
            Email = "layla@example.com",
            Password = "password23",
            DateOfBirth = new DateTime(1995, 11, 8)
        },
        new User
        {
            Id = "5",
            Username = "karim_h",
            Email = "karim@example.com",
            Password = "password123",
            DateOfBirth = new DateTime(1991, 9, 3)
        }
    );

    modelBuilder.Entity<Playlist>().HasData(
        new Playlist { Id = 1, Title = "My Metal Playlist", UserId = "1", CreationDate = new DateTime(2023, 1, 1) },
        new Playlist { Id = 2, Title = "Pop Hits", UserId = "1", CreationDate = new DateTime(2023, 1, 1) },
        new Playlist { Id = 3, Title = "Hip-Hop Essentials", UserId = "1", CreationDate = new DateTime(2023, 1, 1) }
    );

    modelBuilder.Entity<PlaylistSong>().HasData(
        new PlaylistSong { PlaylistId = 1, SongId = 1 },
        new PlaylistSong { PlaylistId = 1, SongId = 2 },
        new PlaylistSong { PlaylistId = 1, SongId = 12 },
        new PlaylistSong { PlaylistId = 2, SongId = 3 },
        new PlaylistSong { PlaylistId = 2, SongId = 4 },
        new PlaylistSong { PlaylistId = 2, SongId = 13 },
        new PlaylistSong { PlaylistId = 3, SongId = 9 },
        new PlaylistSong { PlaylistId = 3, SongId = 10 },
        new PlaylistSong { PlaylistId = 3, SongId = 5 }
    );

    // Additional relationship configurations
    modelBuilder.Entity<Album>()
        .HasOne(a => a.Artist)
        .WithMany(a => a.Albums)
        .HasForeignKey(a => a.ArtistId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Song>()
        .HasOne(s => s.Album)
        .WithMany(a => a.Songs)
        .HasForeignKey(s => s.AlbumId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Artist>()
    .HasMany(a => a.Albums)
    .WithOne(a => a.Artist)
    .HasForeignKey(a => a.ArtistId);
}


    }
}