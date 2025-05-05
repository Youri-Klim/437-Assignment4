using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStreaming.Application.Features.Artists.Queries;
using MusicStreaming.Application.Features.Artists.Commands;
using MusicStreaming.Web.Models;

namespace MusicStreaming.Web.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ArtistsController> _logger;

        public ArtistsController(IMediator mediator, IMapper mapper, ILogger<ArtistsController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var artists = await _mediator.Send(new GetArtistsWithAlbumsQuery());
            var viewModels = _mapper.Map<List<ArtistViewModel>>(artists);
            return View(viewModels);
        }

        public async Task<IActionResult> Create()
        {
            var artists = await _mediator.Send(new GetArtistsQuery());
            ViewBag.Artists = artists;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArtistViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<CreateArtistCommand>(viewModel);
                var result = await _mediator.Send(command);
                
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Failed to create artist");
            }
            
            var artists = await _mediator.Send(new GetArtistsQuery());
            ViewBag.Artists = artists;
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var artist = await _mediator.Send(new GetArtistByIdQuery { Id = id });
            if (artist == null) return NotFound();
            
            var viewModel = _mapper.Map<EditArtistViewModel>(artist);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditArtistViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<UpdateArtistCommand>(viewModel);
                var result = await _mediator.Send(command);
                
                if (result)
                    return RedirectToAction(nameof(Index));
                
                ModelState.AddModelError("", "Failed to update artist");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteArtistCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}