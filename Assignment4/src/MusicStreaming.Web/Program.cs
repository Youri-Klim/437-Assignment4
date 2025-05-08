using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using MusicStreaming.Application;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Application.Features.Songs.Queries;
using MusicStreaming.Application.Mapping;
using MusicStreaming.Core.Entities;
using MusicStreaming.Infrastructure;
using MusicStreaming.Infrastructure.Data;
using MusicStreaming.Infrastructure.Repositories;
using MusicStreaming.Web.Mapping;
using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using MusicStreaming.Application.Services;
using MusicStreaming.Core.Services;
using MusicStreaming.Core.Interfaces;
using MusicStreaming.Core.Events;
using MusicStreaming.Infrastructure.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Infrastructure layer services
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application layer services
builder.Services.AddApplication();

// Register MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(GetSongsQuery).Assembly);
});

// Register AutoMapper - Both profiles
builder.Services.AddAutoMapper(
    typeof(MusicStreaming.Application.Mapping.MappingProfile),
    typeof(MusicStreaming.Web.Mapping.WebMappingProfile)
);

// Register Domain Services
builder.Services.AddScoped<AlbumDomainService>();
builder.Services.AddScoped<ArtistDomainService>();
builder.Services.AddScoped<PlaylistDomainService>();
builder.Services.AddScoped<SongDomainService>();
builder.Services.AddScoped<UserDomainService>();

// Register Application Services
builder.Services.AddScoped<PlaylistService>();
builder.Services.AddScoped<SongService>();
builder.Services.AddScoped<AlbumService>();
builder.Services.AddScoped<ArtistService>();
builder.Services.AddScoped<UserService>();

// Add Identity - UPDATED to use your User class
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MusicStreamingDbContext>();

// Add MVC services with standard validation
builder.Services.AddControllersWithViews();

// Add Razor Pages
builder.Services.AddRazorPages();

// Register FluentValidation validators for DTOs
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.CreateUserDto>, MusicStreaming.Application.Validators.CreateUserDtoValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.UpdateUserDto>, MusicStreaming.Application.Validators.UpdateUserDtoValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.CreatePlaylistDto>, MusicStreaming.Application.Validators.CreatePlaylistDtoValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.UpdatePlaylistDto>, MusicStreaming.Application.Validators.UpdatePlaylistDtoValidator>();

// Add validators for other DTOs used in services
// Album
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.CreateAlbumDto>, MusicStreaming.Application.Validators.CreateAlbumDtoValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.UpdateAlbumDto>, MusicStreaming.Application.Validators.UpdateAlbumDtoValidator>();

// Artist
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.CreateArtistDto>, MusicStreaming.Application.Validators.CreateArtistDtoValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.UpdateArtistDto>, MusicStreaming.Application.Validators.UpdateArtistDtoValidator>();

// Song
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.CreateSongDto>, MusicStreaming.Application.Validators.CreateSongDtoValidator>();
builder.Services.AddScoped<FluentValidation.IValidator<MusicStreaming.Application.DTOs.UpdateSongDto>, MusicStreaming.Application.Validators.UpdateSongDtoValidator>();

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<IDomainEventHandler<PlaylistCreatedEvent>, PlaylistCreatedEventHandler>();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/musicstreaming.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MusicStreamingDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization.");
    }
}

app.Run();