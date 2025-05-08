using FluentValidation;
using MusicStreaming.Application.DTOs;

namespace MusicStreaming.Application.Validators
{
    public class CreateArtistDtoValidator : AbstractValidator<CreateArtistDto>
    {
        public CreateArtistDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
        }
    }
}