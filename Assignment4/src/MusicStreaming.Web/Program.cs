using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicStreaming.Application;
using MusicStreaming.Application.Features.Songs.Queries;
using MusicStreaming.Infrastructure;
using MusicStreaming.Infrastructure.Data;
using Serilog;
using MusicStreaming.Application.Services;
using MusicStreaming.Core.Services;
using MusicStreaming.Core.Interfaces;
using MusicStreaming.Core.Events;
using MusicStreaming.Infrastructure.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MusicStreaming.Infrastructure.Services;
using System.Text;
using MusicStreaming.Infrastructure.Logging;

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

// Add Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MusicStreamingDbContext>();

// Add MVC services with standard validation
builder.Services.AddControllersWithViews();

// Add Razor Pages
builder.Services.AddRazorPages();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    // Use cookies as the default scheme for browser requests
    options.DefaultAuthenticateScheme = "Identity.Application";
    options.DefaultChallengeScheme = "Identity.Application";
    options.DefaultScheme = "Identity.Application";
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? 
                throw new InvalidOperationException("JWT:Secret is not configured in appsettings.json")))
    };
});

// Register the JWT Service
builder.Services.AddScoped<JwtService>();

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

builder.Services.AddLoggingServices(builder.Configuration);

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