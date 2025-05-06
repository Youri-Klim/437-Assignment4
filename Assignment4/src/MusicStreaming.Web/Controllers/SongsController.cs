using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Features.Songs.Queries;
using MusicStreaming.Application.Features.Songs.Commands;
using MusicStreaming.Application.Features.Albums.Queries; // Added for GetAlbumsQuery
using MusicStreaming.Application.Features.Playlists.Queries;
using MusicStreaming.Application.Features.Playlists.Commands;
using MusicStreaming.Web.Models;
using MusicStreaming.Application.DTOs; // Added for AlbumDto
using AutoMapper;
using System.Collections.Generic;

namespace MusicStreaming.Web.Controllers
{
    public class SongsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SongsController> _logger;
        private readonly IMapper _mapper;

        public SongsController(IMediator mediator, ILogger<SongsController> logger, IMapper mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var songs = await _mediator.Send(new GetSongsQuery());
            var songViewModels = _mapper.Map<List<SongViewModel>>(songs);
            return View(songViewModels);
        }

        public async Task<IActionResult> Create()
        {
            var albums = await _mediator.Send(new GetAlbumsQuery());
            ViewBag.Albums = albums ?? new List<AlbumDto>(); // Fixed nullability warning
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSongViewModel songViewModel)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<CreateSongCommand>(songViewModel);
                var result = await _mediator.Send(command);
                
                if (result > 0) // Success - got the new song ID
                    return RedirectToAction("Index");
                
                ModelState.AddModelError("", "Failed to create song");
            }
            
            // If we got here, something failed - redisplay form
            var albums = await _mediator.Send(new GetAlbumsQuery());
            ViewBag.Albums = albums ?? new List<AlbumDto>(); // Fixed nullability warning
            return View(songViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var song = await _mediator.Send(new GetSongByIdQuery { Id = id });
            if (song == null) return NotFound();
            
            var songViewModel = _mapper.Map<EditSongViewModel>(song);
            return View(songViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSongViewModel songViewModel)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<UpdateSongCommand>(songViewModel);
                var result = await _mediator.Send(command);
                
                if (result) // Success
                    return RedirectToAction("Index");
                    
                ModelState.AddModelError("", "Failed to update song");
            }
            return View(songViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteSongCommand { Id = id });
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddToPlaylist(int id)
        {
            var song = await _mediator.Send(new GetSongByIdQuery { Id = id });
            if (song == null) return NotFound();

            // In a real app, get the current user ID - for now using hardcoded value
            var playlists = await _mediator.Send(new GetPlaylistsByUserQuery { UserId = "1" });
            ViewBag.Playlists = playlists ?? new List<PlaylistDto>(); // Fixed nullability warning
            
            return View(_mapper.Map<SongViewModel>(song));
        }

        [HttpPost]
        public async Task<IActionResult> AddToPlaylist(int songId, int playlistId)
        {
            var command = new AddSongToPlaylistCommand
            {
                SongId = songId,
                PlaylistId = playlistId
            };
            
            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var song = await _mediator.Send(new GetSongByIdQuery { Id = id });
                if (song == null) return NotFound();
                
                var songViewModel = _mapper.Map<SongDetailViewModel>(song);
                return View(songViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving song details for ID {SongId}", id);
                return View("Error", new ErrorViewModel { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorMessage = "Unable to retrieve song details. Please try again later."
                });
            }
        }
    }
}