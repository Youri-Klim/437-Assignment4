using FluentValidation;
using MediatR;
using MusicStreaming.Application.Services;
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
        private readonly ArtistService _artistService;
        
        public DeleteArtistCommandHandler(ArtistService artistService)
        {
            _artistService = artistService;
        }
        
        public async Task<bool> Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _artistService.DeleteAsync(request.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}