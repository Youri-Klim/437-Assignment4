using FluentValidation;
using MusicStreaming.Application.DTOs;

namespace MusicStreaming.Application.Validators
{
    public class CreateAlbumDtoValidator : AbstractValidator<CreateAlbumDto>
    {
        public CreateAlbumDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

                
            RuleFor(x => x.ArtistId)
                .GreaterThan(0).WithMessage("A valid artist ID is required");
        }
    }
}