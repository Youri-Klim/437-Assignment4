using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Artists.Commands
{
    public class CreateArtistCommand : IRequest<int>
    {
        public required string Name { get; set; }
        public required string Genre { get; set; }
    }

    public class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
    {
        public CreateArtistCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
                
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters");
        }
    }

    public class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, int>
    {
        private readonly IArtistRepository _artistRepository;
        
        public CreateArtistCommandHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }
        
        public async Task<int> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
        {
            var artistDto = new CreateArtistDto
            {
                Name = request.Name,
                Genre = request.Genre
            };
            
            return await _artistRepository.AddAsync(artistDto);
        }
    }
}