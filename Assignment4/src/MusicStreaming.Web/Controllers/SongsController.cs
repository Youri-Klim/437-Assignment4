using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Features.Songs.Queries;
using MusicStreaming.Application.Features.Songs.Commands;
using MusicStreaming.Application.Features.Albums.Queries;
using MusicStreaming.Application.Features.Playlists.Queries;
using MusicStreaming.Application.Features.Playlists.Commands;
using MusicStreaming.Web.ViewModels;
using MusicStreaming.Application.DTOs;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

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
            var model = new CreateSongViewModel
            {
                Title = string.Empty, 
                Genre = string.Empty,    
                ReleaseDate = DateTime.Today 
            };
    
  
            var albums = await _mediator.Send(new GetAlbumsQuery());
            model.AlbumOptions = albums.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Title
            }).ToList();
    
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSongViewModel songViewModel)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<CreateSongCommand>(songViewModel);
                var result = await _mediator.Send(command);
                
                if (result > 0)
                    return RedirectToAction("Index");
                
                ModelState.AddModelError("", "Failed to create song");
            }
        
            var albums = await _mediator.Send(new GetAlbumsQuery());
            songViewModel.AlbumOptions = albums.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Title
            }).ToList();
            
            return View(songViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var song = await _mediator.Send(new GetSongByIdQuery { Id = id });
                
                if (song == null)
                {
                    return NotFound();
                }
                
                var viewModel = _mapper.Map<EditSongViewModel>(song);
                
              
                var albums = await _mediator.Send(new GetAlbumsQuery());
                viewModel.AlbumOptions = albums.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Title
                }).ToList();
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving song for edit with ID {Id}", id);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSongViewModel songViewModel)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<UpdateSongCommand>(songViewModel);
                var result = await _mediator.Send(command);
                
                if (result) 
                    return RedirectToAction("Index");
                    
                ModelState.AddModelError("", "Failed to update song");
            }
            
      
            var albums = await _mediator.Send(new GetAlbumsQuery());
            songViewModel.AlbumOptions = albums.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Title
            }).ToList();
            
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


    var playlists = await _mediator.Send(new GetPlaylistsByUserQuery { UserId = "1" });
    ViewBag.Playlists = playlists ?? new List<PlaylistDto>();
    
    return View(_mapper.Map<SongViewModel>(song));
}


[HttpPost]
public async Task<IActionResult> AddToPlaylist(int songId, int playlistId)
{
    try
    {
        _logger.LogInformation("Starting AddToPlaylist operation");
        _logger.LogInformation("Parameters - SongId: {SongId}, PlaylistId: {PlaylistId}", songId, playlistId);
        
 
        var song = await _mediator.Send(new GetSongByIdQuery { Id = songId });
        if (song == null)
        {
            _logger.LogWarning("Song with ID {SongId} not found", songId);
            TempData["ErrorMessage"] = "Song not found";
            return RedirectToAction("Index");
        }
        
 
        var playlist = await _mediator.Send(new GetPlaylistByIdQuery { Id = playlistId });
        if (playlist == null)
        {
            _logger.LogWarning("Playlist with ID {PlaylistId} not found", playlistId);
            TempData["ErrorMessage"] = "Playlist not found";
            return RedirectToAction("Index");
        }
        
        _logger.LogInformation("Sending AddSongToPlaylist command");
        var command = new AddSongToPlaylistCommand
        {
            SongId = songId,
            PlaylistId = playlistId
        };
        
        await _mediator.Send(command);
        
        _logger.LogInformation("Song {SongId} successfully added to playlist {PlaylistId}", songId, playlistId);
        TempData["SuccessMessage"] = "Song added to playlist successfully";
        return RedirectToAction("Details", "Playlists", new { id = playlistId });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error adding song {SongId} to playlist {PlaylistId}", songId, playlistId);
        TempData["ErrorMessage"] = "Failed to add song to playlist: " + ex.Message;
        return RedirectToAction("Index");
    }
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