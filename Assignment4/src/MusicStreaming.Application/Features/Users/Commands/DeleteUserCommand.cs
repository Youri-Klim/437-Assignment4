using FluentValidation;
using MediatR;
using MusicStreaming.Application.Services;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required");
        }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly UserService _userService;
        
        public DeleteUserCommandHandler(UserService userService)
        {
            _userService = userService;
        }
        
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.DeleteAsync(request.Id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}