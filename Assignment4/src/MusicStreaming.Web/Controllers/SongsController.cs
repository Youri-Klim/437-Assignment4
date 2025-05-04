namespace MusicStreaming.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public SongsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public async Task<ActionResult<PagedResult<SongDto>>> GetSongs([FromQuery] GetSongsQuery query)
        {
            return await _mediator.Send(query);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<SongDto>> GetSong(int id)
        {
            var result = await _mediator.Send(new GetSongByIdQuery { Id = id });
            
            if (result == null)
                return NotFound();
                
            return result;
        }
        
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<int>> CreateSong(CreateSongCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetSong), new { id }, null);
        }
        
        [HttpGet("byArtist/{artistId}")]
        public async Task<ActionResult<List<SongDto>>> GetSongsByArtist(int artistId)
        {
            var result = await _mediator.Send(new GetSongsByArtistQuery { ArtistId = artistId });
            return result;
        }
        
        [HttpGet("byAlbum/{albumId}")]
        public async Task<ActionResult<List<SongDto>>> GetSongsByAlbum(int albumId)
        {
            var result = await _mediator.Send(new GetSongsByAlbumQuery { AlbumId = albumId });
            return result;
        }
        
        // Other endpoints...
    }
}