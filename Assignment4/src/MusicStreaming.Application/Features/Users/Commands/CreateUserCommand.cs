using FluentValidation;
using MediatR;
using MusicStreaming.Application.DTOs;
using MusicStreaming.Application.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStreaming.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<string>
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");
                
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");
                
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly UserService _userService;
        
        public CreateUserCommandHandler(UserService userService)
        {
            _userService = userService;
        }
        
        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userDto = new CreateUserDto
            {
                Username = request.Username,
                Email = request.Email
            };
            
            return await _userService.AddAsync(userDto);
        }
    }
}