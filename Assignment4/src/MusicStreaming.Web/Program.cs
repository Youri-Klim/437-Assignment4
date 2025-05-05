using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicStreaming.Infrastructure.Persistence;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Infrastructure.Repositories;
using MusicStreaming.Web.Mapping;
using MusicStreaming.Application.Features.Songs.Queries;
using MusicStreaming.Core.Entities;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MusicStreaming.Application.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

// Add Identity services (if you're using ASP.NET Core Identity)
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

// Register MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(GetSongsQuery).Assembly);
});

// Register AutoMapper - UPDATED TO INCLUDE BOTH PROFILES
builder.Services.AddAutoMapper(
    typeof(MusicStreaming.Application.Mapping.MappingProfile),  // Application profile
    typeof(MusicStreaming.Web.Mapping.WebMappingProfile)        // Web profile
);

// Register Fluent Validation
builder.Services.AddValidatorsFromAssembly(typeof(GetSongsQuery).Assembly);
builder.Services.AddFluentValidationAutoValidation();

// Register repositories
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add MVC services
builder.Services.AddControllersWithViews();

// Add Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// The rest of the file remains unchanged