using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Features.Playlists.Queries;
using MusicStreaming.Application.Features.Playlists.Commands;
using MusicStreaming.Web.ViewModels;

namespace MusicStreaming.Web.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<PlaylistsController> _logger;

        public PlaylistsController(IMediator mediator, IMapper mapper, ILogger<PlaylistsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var playlists = await _mediator.Send(new GetPlaylistsQuery());
            var viewModels = _mapper.Map<List<PlaylistViewModel>>(playlists);
            return View(viewModels);
        }

        public IActionResult Create()
{
    var model = new CreatePlaylistViewModel
    {
        Title = string.Empty,
        UserId = "1" 
    };
    return View(model);
}

[HttpPost]
public async Task<IActionResult> Create(CreatePlaylistViewModel viewModel)
{
    if (ModelState.IsValid)
    {
        var command = _mapper.Map<CreatePlaylistCommand>(viewModel);
        var result = await _mediator.Send(command);
        
        if (result > 0)
            return RedirectToAction(nameof(Index));
            
        ModelState.AddModelError("", "Failed to create playlist");
    }
    return View(viewModel);
}

        public async Task<IActionResult> Edit(int id)
        {
            var playlist = await _mediator.Send(new GetPlaylistWithSongsQuery { Id = id });
            if (playlist == null) return NotFound();
            
            var viewModel = _mapper.Map<EditPlaylistViewModel>(playlist);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string title)
        {
            var command = new UpdatePlaylistCommand
            {
                Id = id,
                Title = title
            };
            
            var result = await _mediator.Send(command);
            
            if (!result)
                return NotFound();
                
            return RedirectToAction("Edit", new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var playlist = await _mediator.Send(new GetPlaylistByIdQuery { Id = id });
            if (playlist == null) return NotFound();
            
            var viewModel = _mapper.Map<PlaylistViewModel>(playlist);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeletePlaylistCommand { Id = id });
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSong(int playlistId, int songId)
        {
            var command = new RemoveSongFromPlaylistCommand
            {
                PlaylistId = playlistId,
                SongId = songId
            };
            
            await _mediator.Send(command);
            return RedirectToAction("Edit", new { id = playlistId });
        }
        
        public async Task<IActionResult> Details(int id)
{
    try
    {
        var playlist = await _mediator.Send(new GetPlaylistWithSongsQuery { Id = id });
        
        if (playlist == null)
            return NotFound();
            
        _logger.LogInformation("Playlist {PlaylistId} loaded with {SongCount} songs", 
            id, playlist.Songs?.Count ?? 0);

        var viewModel = _mapper.Map<PlaylistDetailViewModel>(playlist);
        return View(viewModel);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving playlist with ID {Id}", id);
        return View("Error");
    }
}
    }
}