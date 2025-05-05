using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Users.Queries
{
    public class GetUsersQuery : IRequest<IReadOnlyList<UserDto>>
    {
        // No parameters needed
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserDto>>
    {
        private readonly IUserRepository _userRepository;
        
        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.ListAllAsync();
        }
    }
}