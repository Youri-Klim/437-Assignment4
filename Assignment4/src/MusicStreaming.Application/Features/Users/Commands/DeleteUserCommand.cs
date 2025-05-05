using MediatR;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public string Id { get; set; }
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
            return await _userRepository.DeleteAsync(request.Id);
        }
    }
}