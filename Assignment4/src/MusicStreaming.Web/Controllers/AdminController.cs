using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Features.Users.Queries;
using MusicStreaming.Application.Features.Users.Commands;
using MusicStreaming.Application.Features.Songs.Queries;
using MusicStreaming.Application.Features.Songs.Commands;
using MusicStreaming.Application.Features.Albums.Queries;
using MusicStreaming.Application.Features.Albums.Commands;
using MusicStreaming.Application.Features.Artists.Queries;
using MusicStreaming.Application.Features.Artists.Commands;
using MusicStreaming.Web.ViewModels;
using AutoMapper;

namespace MusicStreaming.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IMediator mediator, IMapper mapper, ILogger<AdminController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Users = await _mediator.Send(new GetUsersQuery());
                ViewBag.Songs = await _mediator.Send(new GetSongsQuery());
                ViewBag.Artists = await _mediator.Send(new GetArtistsWithAlbumsQuery());
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard data");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "Unable to load admin dashboard. Database access error."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                if (id == "1") // Assuming "1" is the admin user ID
                {
                    ModelState.AddModelError("", "Admin user cannot be deleted.");
                    return RedirectToAction("Index");
                }

                var result = await _mediator.Send(new DeleteUserCommand { Id = id });
                
                if (!result)
                {
                    TempData["ErrorMessage"] = "User not found or could not be deleted.";
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                TempData["ErrorMessage"] = "Could not delete this user. They may have related data.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> CreateAlbum()
        {
            try
            {
                ViewBag.Artists = await _mediator.Send(new GetArtistsQuery());
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading artists for album creation");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "Unable to load artist data for album creation."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum(CreateAlbumWithSongViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrEmpty(viewModel.SongTitle))
                {
                    ModelState.AddModelError("", "An album must have at least one song.");
                    ViewBag.Artists = await _mediator.Send(new GetArtistsQuery());
                    return View(viewModel);
                }
                
                if (!ModelState.IsValid)
                {
                    ViewBag.Artists = await _mediator.Send(new GetArtistsQuery());
                    return View(viewModel);
                }

                var command = new CreateAlbumWithSongCommand
                {
                    Title = viewModel.Title,
                    ReleaseYear = viewModel.ReleaseYear,
                    Genre = viewModel.Genre,
                    ArtistId = viewModel.ArtistId,
                    SongTitle = viewModel.SongTitle,
                    SongDurationInSeconds = viewModel.SongDurationInSeconds,
                    SongGenre = viewModel.SongGenre,
                    SongReleaseDate = DateTime.Now // Default to current date
                };

                var result = await _mediator.Send(command);
                
                if (result > 0)
                    return RedirectToAction("Index");
                
                ModelState.AddModelError("", "Failed to create album with song.");
                ViewBag.Artists = await _mediator.Send(new GetArtistsQuery());
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating album with song: {Message}", ex.Message);
                ModelState.AddModelError("", "Failed to create album. Server error.");
                ViewBag.Artists = await _mediator.Send(new GetArtistsQuery());
                return View(viewModel);
            }
        }

        public async Task<IActionResult> CreateSong()
        {
            try
            {
                ViewBag.Albums = await _mediator.Send(new GetAlbumsQuery());
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading albums for song creation");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "Unable to load album data for song creation."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong(CreateSongViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = _mapper.Map<CreateSongCommand>(viewModel);
                    var result = await _mediator.Send(command);
                    
                    if (result > 0)
                        return RedirectToAction("Index");
                    
                    ModelState.AddModelError("", "Failed to create song.");
                }
                
                ViewBag.Albums = await _mediator.Send(new GetAlbumsQuery());
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating song: {Message}", ex.Message);
                ModelState.AddModelError("", "Failed to create song. Server error.");
                ViewBag.Albums = await _mediator.Send(new GetAlbumsQuery());
                return View(viewModel);
            }
        }

        public IActionResult CreateArtist()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtist(CreateArtistViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var command = _mapper.Map<CreateArtistCommand>(viewModel);
                    var result = await _mediator.Send(command);
                    
                    if (result > 0)
                        return RedirectToAction("Index");
                    
                    ModelState.AddModelError("", "Failed to create artist.");
                }
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating artist: {Message}", ex.Message);
                ModelState.AddModelError("", "Failed to create artist. Server error.");
                return View(viewModel);
            }
        }
    }
}