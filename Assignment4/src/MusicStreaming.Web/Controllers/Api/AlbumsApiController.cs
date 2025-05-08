using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicStreaming.Application.Common.APIWrapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Features.Albums.Queries;
using MusicStreaming.Application.Features.Albums.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicStreaming.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AlbumsApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Albums
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AlbumDto>>>> GetAll()
        {
            var albums = await _mediator.Send(new GetAlbumsQuery());
            return Ok(ApiResponse<IEnumerable<AlbumDto>>.SuccessResponse(albums, "Albums retrieved successfully"));
        }

        // GET: api/Albums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AlbumDto>>> GetById(int id)
        {
            var album = await _mediator.Send(new GetAlbumByIdQuery { Id = id });
            
            if (album == null)
                return NotFound(ApiResponse<AlbumDto>.ErrorResponse("Album not found"));
                
            return Ok(ApiResponse<AlbumDto>.SuccessResponse(album, "Album retrieved successfully"));
        }
    }
}