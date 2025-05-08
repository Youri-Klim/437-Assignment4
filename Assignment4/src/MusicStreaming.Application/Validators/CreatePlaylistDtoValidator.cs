using FluentValidation;
using MusicStreaming.Application.DTOs;

namespace MusicStreaming.Application.Validators
{
    public class CreatePlaylistDtoValidator : AbstractValidator<CreatePlaylistDto>
    {
        public CreatePlaylistDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
                
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }
}