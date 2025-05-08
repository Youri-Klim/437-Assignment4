using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicStreaming.Application.Common.APIWrapper;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Features.Songs.Queries;
using MusicStreaming.Application.Features.Songs.Commands;
using MusicStreaming.Application.Features.Playlists.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MusicStreaming.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsApiController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SongsApiController> _logger;

        public SongsApiController(IMediator mediator, ILogger<SongsApiController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/Songs
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SongDto>>>> GetAll()
        {
            var songs = await _mediator.Send(new GetSongsQuery());
            return Ok(ApiResponse<IEnumerable<SongDto>>.SuccessResponse(songs, "Songs retrieved successfully"));
        }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SongDto>>> GetById(int id)
        {
            var song = await _mediator.Send(new GetSongByIdQuery { Id = id });
            
            if (song == null)
                return NotFound(ApiResponse<SongDto>.ErrorResponse("Song not found"));
                
            return Ok(ApiResponse<SongDto>.SuccessResponse(song, "Song retrieved successfully"));
        }

        // POST: api/Songs
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreateSongCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(ApiResponse<int>.SuccessResponse(result, "Song created successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error creating song");
                return BadRequest(ApiResponse<int>.ErrorResponse("Failed to create song", new List<string> { ex.Message }));
            }
        }

        // PUT: api/Songs/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Update(int id, UpdateSongCommand command)
        {
            if (id != command.Id)
                return BadRequest(ApiResponse<bool>.ErrorResponse("ID mismatch"));
                
            try
            {
                var result = await _mediator.Send(command);
                
                if (!result)
                    return NotFound(ApiResponse<bool>.ErrorResponse("Song not found"));
                    
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Song updated successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating song");
                return BadRequest(ApiResponse<bool>.ErrorResponse("Failed to update song", new List<string> { ex.Message }));
            }
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteSongCommand { Id = id });
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Song deleted successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error deleting song");
                return BadRequest(ApiResponse<bool>.ErrorResponse("Failed to delete song", new List<string> { ex.Message }));
            }
        }
        
        // POST: api/Songs/{songId}/playlists/{playlistId}
        [HttpPost("{songId}/playlists/{playlistId}")]
        public async Task<ActionResult<ApiResponse<bool>>> AddToPlaylist(int songId, int playlistId)
        {
            try
            {
                _logger.LogInformation("API: Adding song {SongId} to playlist {PlaylistId}", songId, playlistId);
                
                var command = new AddSongToPlaylistCommand
                {
                    SongId = songId,
                    PlaylistId = playlistId
                };
                
                await _mediator.Send(command);
                
                return Ok(ApiResponse<bool>.SuccessResponse(true, "Song added to playlist successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error adding song {SongId} to playlist {PlaylistId}", songId, playlistId);
                return BadRequest(ApiResponse<bool>.ErrorResponse(
                    "Failed to add song to playlist", 
                    new List<string> { ex.Message }));
            }
        }
    }
}