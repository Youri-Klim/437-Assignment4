using AutoMapper;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Interfaces.Repositories;
using MusicStreaming.Core.Entities;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper _mapper;
        
        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        
        public async Task<IReadOnlyList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // Get users from repository
            var users = await _userRepository.GetAllAsync();
            
            // Map to DTOs and convert to IReadOnlyList
            var userDtos = _mapper.Map<List<UserDto>>(users);
            
            return userDtos;
        }
    }
}