using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Features.Playlists.Queries;
using MusicStreaming.Application.Features.Playlists.Commands;
using MusicStreaming.Web.Models;

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePlaylistViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = new CreatePlaylistCommand
                    {
                        Title = viewModel.Title,
                        UserId = "1" // Hardcoded for now, would normally come from authentication
                    };
                    
                    var result = await _mediator.Send(command);
                    
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
                        
                    ModelState.AddModelError("", "Failed to create playlist");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating playlist");
                    ModelState.AddModelError("", "Error creating playlist: " + ex.Message);
                }
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
    }
}