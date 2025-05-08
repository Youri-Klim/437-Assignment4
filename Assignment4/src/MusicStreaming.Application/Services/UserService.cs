using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Exceptions;
using MusicStreaming.Core.Entities;
using MusicStreaming.Core.Extensions;
using MusicStreaming.Core.Interfaces.Repositories;
using MusicStreaming.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserDomainService _userDomainService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUserDto> _createValidator;
        private readonly IValidator<UpdateUserDto> _updateValidator;

        public UserService(
            IUserRepository userRepository,
            UserDomainService userDomainService,
            IMapper mapper,
            IValidator<CreateUserDto> createValidator,
            IValidator<UpdateUserDto> updateValidator)
        {
            _userRepository = userRepository;
            _userDomainService = userDomainService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IReadOnlyList<UserDto>> ListAllAsync()
        {
            var users = await _userRepository.ListAllAsync();
            return _mapper.Map<IReadOnlyList<UserDto>>(users);
        }

        public async Task<string> AddAsync(CreateUserDto userDto)
        {
            // FluentValidation
            var validationResult = await _createValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            // Create a new User with correct properties (remove PasswordHash)
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email
            };
            
            // Domain validation - convert to FluentValidation format
            var domainErrors = _userDomainService.ValidateUser(user);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            return await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(UpdateUserDto userDto)
        {
            // FluentValidation
            var validationResult = await _updateValidator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            
            var user = await _userRepository.GetByIdAsync(userDto.Id);
            if (user == null)
                throw new NotFoundException($"User with ID {userDto.Id} not found");
                
            // Update user properties with correct method signature (2 params)
            user.UpdateUserDetails(
                userDto.Username, 
                userDto.Email
            );
            
            // Domain validation
            var domainErrors = _userDomainService.ValidateUser(user);
            if (domainErrors.Count > 0)
            {
                var failures = domainErrors.Select(error => 
                    new ValidationFailure("Domain", error)).ToList();
                throw new ValidationException(failures);
            }
            
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string id)
{
    if (string.IsNullOrEmpty(id))
        throw new ArgumentException("User ID cannot be null or empty", nameof(id));
    
    var user = await _userRepository.GetByIdAsync(id);
    if (user == null)
        throw new NotFoundException($"User with ID {id} not found");
        
    await _userRepository.DeleteAsync(id);
}
    }
}