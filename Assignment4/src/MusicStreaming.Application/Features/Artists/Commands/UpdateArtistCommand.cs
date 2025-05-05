using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Artists.Commands
{
    public class UpdateArtistCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
    }

    public class UpdateArtistCommandValidator : AbstractValidator<UpdateArtistCommand>
    {
        public UpdateArtistCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid artist ID is required");
                
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
                
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters");
        }
    }

    public class UpdateArtistCommandHandler : IRequestHandler<UpdateArtistCommand, bool>
    {
        private readonly IArtistRepository _artistRepository;
        
        public UpdateArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }
        
        public async Task<bool> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
        {
            var artistDto = new UpdateArtistDto
            {
                Id = request.Id,
                Name = request.Name,
                Genre = request.Genre
            };
            
            await _artistRepository.UpdateAsync(artistDto);
            return true;
        }
    }
}