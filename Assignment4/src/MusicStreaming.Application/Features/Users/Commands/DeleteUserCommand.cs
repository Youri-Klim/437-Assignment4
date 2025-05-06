using MediatR;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public required string Id { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        
        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Since DeleteAsync returns void, we'll call it and then return true 
            // to indicate successful completion
            await _userRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}