using FluentValidation;
using MediatR;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Artists.Commands
{
    public class DeleteArtistCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteArtistCommandValidator : AbstractValidator<DeleteArtistCommand>
    {
        public DeleteArtistCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid artist ID is required");
        }
    }

    public class DeleteArtistCommandHandler : IRequestHandler<DeleteArtistCommand, bool>
    {
        private readonly IArtistRepository _artistRepository;
        
        public DeleteArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }
        
        public async Task<bool> Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
        {
            await _artistRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}