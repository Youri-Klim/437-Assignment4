using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using MusicStreaming.Application;
using MusicStreaming.Application.Interfaces.Repositories;
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

// REMOVED: FluentValidation configuration to fix errors

// Add Identity - UPDATED to use your User class
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<MusicStreamingDbContext>();

// Add MVC services with standard validation
builder.Services.AddControllersWithViews();

// Add Razor Pages
builder.Services.AddRazorPages();

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

// Seed database if needed
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