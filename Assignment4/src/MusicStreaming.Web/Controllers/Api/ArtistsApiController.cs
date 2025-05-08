using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicStreaming.Application.Common.APIWrapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Features.Artists.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArtistsApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ArtistDto>>>> GetAll()
        {
            var artists = await _mediator.Send(new GetArtistsQuery());
            return Ok(ApiResponse<IEnumerable<ArtistDto>>.SuccessResponse(artists));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ArtistDto>>> GetById(int id)
        {
            var artist = await _mediator.Send(new GetArtistByIdQuery { Id = id });
            
            if (artist == null)
                return NotFound(ApiResponse<ArtistDto>.ErrorResponse("Artist not found"));
                
            return Ok(ApiResponse<ArtistDto>.SuccessResponse(artist));
        }
    }
}