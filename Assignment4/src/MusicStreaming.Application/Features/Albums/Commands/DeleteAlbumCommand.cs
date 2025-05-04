using FluentValidation;
using MediatR;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Albums.Commands
{
    public class DeleteAlbumCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteAlbumCommandValidator : AbstractValidator<DeleteAlbumCommand>
    {
        public DeleteAlbumCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid album ID is required");
        }
    }

    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand, bool>
    {
        private readonly IAlbumRepository _albumRepository;
        
        public DeleteAlbumCommandHandler(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }
        
        public async Task<bool> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            await _albumRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}