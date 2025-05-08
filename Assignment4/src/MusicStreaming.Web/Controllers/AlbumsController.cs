using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Features.Albums.Queries;
using MusicStreaming.Application.Features.Albums.Commands;
using MusicStreaming.Application.Features.Artists.Queries;
using MusicStreaming.Web.ViewModels;
using MusicStreaming.Application.DTOs;

namespace MusicStreaming.Web.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IMediator mediator, IMapper mapper, ILogger<AlbumsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var albums = await _mediator.Send(new GetAlbumsQuery());
            var viewModels = _mapper.Map<List<AlbumViewModel>>(albums);
            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var artists = await _mediator.Send(new GetArtistsQuery());
                ViewBag.Artists = artists ?? new List<ArtistDto>();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading artists for album creation");
                ModelState.AddModelError("", "Could not load artist data. Please try again.");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAlbumViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _mapper.Map<CreateAlbumCommand>(viewModel);
                    var result = await _mediator.Send(command);
                    
                    if (result > 0)
                        return RedirectToAction(nameof(Index));
                    
                    ModelState.AddModelError("", "Failed to create album");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating album");
                    ModelState.AddModelError("", "An error occurred while creating the album.");
                }
            }
            
            var artists = await _mediator.Send(new GetArtistsQuery());
            ViewBag.Artists = artists ?? new List<ArtistDto>(); 
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var album = await _mediator.Send(new GetAlbumByIdQuery { Id = id });
                if (album == null) return NotFound();
                
                var viewModel = _mapper.Map<EditAlbumViewModel>(album);
                
                var artists = await _mediator.Send(new GetArtistsQuery());
                ViewBag.Artists = artists ?? new List<ArtistDto>(); 
                
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading album for editing");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditAlbumViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = _mapper.Map<UpdateAlbumCommand>(viewModel);
                    var result = await _mediator.Send(command);
                    
                    if (result)
                        return RedirectToAction(nameof(Index));
                    
                    ModelState.AddModelError("", "Failed to update album");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating album");
                    ModelState.AddModelError("", "An error occurred while updating the album.");
                }
            }
            
            var artists = await _mediator.Send(new GetArtistsQuery());
            ViewBag.Artists = artists ?? new List<ArtistDto>(); 
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteAlbumCommand { Id = id });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting album");
                TempData["ErrorMessage"] = "Failed to delete the album.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var album = await _mediator.Send(new GetAlbumWithSongsQuery { Id = id });
                if (album == null) return NotFound();
                
                var viewModel = _mapper.Map<AlbumDetailViewModel>(album);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading album details");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}