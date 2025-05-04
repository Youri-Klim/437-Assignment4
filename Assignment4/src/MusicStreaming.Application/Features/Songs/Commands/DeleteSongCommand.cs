using FluentValidation;
using MediatR;
using MusicStreaming.Application.Interfaces.Repositories;
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
        private readonly ISongRepository _songRepository;
        
        public DeleteSongCommandHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }
        
        public async Task<bool> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
        {
            await _songRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}