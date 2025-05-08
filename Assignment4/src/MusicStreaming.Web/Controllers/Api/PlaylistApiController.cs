using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicStreaming.Application.Common.APIWrapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Features.Playlists.Queries;
using MusicStreaming.Application.Features.Playlists.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MusicStreaming.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PlaylistsApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Playlists
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PlaylistDto>>>> GetAll()
        {
            var playlists = await _mediator.Send(new GetPlaylistsQuery());
            return Ok(ApiResponse<IEnumerable<PlaylistDto>>.SuccessResponse(playlists, "Playlists retrieved successfully"));
        }

        // GET: api/Playlists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PlaylistDto>>> GetById(int id)
        {
            var playlist = await _mediator.Send(new GetPlaylistByIdQuery { Id = id });
            
            if (playlist == null)
                return NotFound(ApiResponse<PlaylistDto>.ErrorResponse("Playlist not found"));
                
            return Ok(ApiResponse<PlaylistDto>.SuccessResponse(playlist, "Playlist retrieved successfully"));
        }
        
        // GET: api/Playlists/5/songs
        [HttpGet("{id}/songs")]
        public async Task<ActionResult<ApiResponse<PlaylistDto>>> GetWithSongs(int id)
        {
            var playlist = await _mediator.Send(new GetPlaylistWithSongsQuery { Id = id });
            
            if (playlist == null)
                return NotFound(ApiResponse<PlaylistDto>.ErrorResponse("Playlist not found"));
                
            return Ok(ApiResponse<PlaylistDto>.SuccessResponse(playlist, "Playlist with songs retrieved successfully"));
        }

        // POST: api/Playlists
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreatePlaylistCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<int>.SuccessResponse(result, "Playlist created successfully"));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<int>.ErrorResponse("Failed to create playlist", new List<string> { ex.Message }));
            }
        }

        // PUT: api/Playlists/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, UpdatePlaylistCommand command)
        {
            if (id != command.Id)
                return BadRequest(ApiResponse<bool>.ErrorResponse("ID mismatch"));
                
            try
            {
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Playlist not found"));
                    
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Playlist updated successfully"));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Failed to update playlist", new List<string> { ex.Message }));
            }
        }

        // DELETE: api/Playlists/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeletePlaylistCommand { Id = id });
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Playlist deleted successfully"));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Failed to delete playlist", new List<string> { ex.Message }));
            }
        }
        
        // POST: api/Playlists/5/songs/remove
        [HttpPost("{playlistId}/songs/remove")]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveSong(int playlistId, [FromBody] RemoveSongFromPlaylistCommand command)
        {
            if (playlistId != command.PlaylistId)
                return BadRequest(ApiResponse<bool>.ErrorResponse("Playlist ID mismatch"));
                
            try
            {
                await _mediator.Send(command);
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Song removed from playlist successfully"));
            }
            catch (System.Exception ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse("Failed to remove song from playlist", 
                    new List<string> { ex.Message }));
            }
        }
    }
}