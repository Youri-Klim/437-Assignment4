using FluentValidation;
using MediatR;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Songs.Commands
{
    public class DeleteSongCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteSongCommandValidator : AbstractValidator<DeleteSongCommand>
    {
        public DeleteSongCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid song ID is required");
        }
    }

    public class DeleteSongCommandHandler : IRequestHandler<DeleteSongCommand, bool>
    {
        private readonly SongService _songService;
        
        public DeleteSongCommandHandler(SongService songService)
        {
            _songService = songService;
        }
        
        public async Task<bool> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _songService.DeleteAsync(request.Id);
                return true;
            }
            catch
            {
                // Consider logging the exception here
                return false;
            }
        }
    }
}