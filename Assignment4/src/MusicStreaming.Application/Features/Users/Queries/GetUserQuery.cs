using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
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
        private readonly UserService _userService;
        
        public GetUsersQueryHandler(UserService userService)
        {
            _userService = userService;
        }
        
        public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userService.ListAllAsync();
        }
    }
}