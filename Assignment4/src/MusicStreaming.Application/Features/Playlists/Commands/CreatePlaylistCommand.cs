using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Playlists.Commands
{
    public class CreatePlaylistCommand : IRequest<int>
    {
        public required string Title { get; set; }
        public required string UserId { get; set; }
    }

    public class CreatePlaylistCommandValidator : AbstractValidator<CreatePlaylistCommand>
    {
        public CreatePlaylistCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
                
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }

    public class CreatePlaylistCommandHandler : IRequestHandler<CreatePlaylistCommand, int>
    {
        private readonly IPlaylistRepository _playlistRepository;
        
        public CreatePlaylistCommandHandler(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
        
        public async Task<int> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
        {
            var playlistDto = new CreatePlaylistDto
            {
                Title = request.Title,
                UserId = request.UserId
            };
                
            return await _playlistRepository.AddAsync(playlistDto);
        }
    }
}