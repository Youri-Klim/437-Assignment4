using FluentValidation;
using MusicStreaming.Application.DTOs;
using System;

namespace MusicStreaming.Application.Validators
{
    public class CreateSongDtoValidator : AbstractValidator<CreateSongDto>
    {
        public CreateSongDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
                
            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 seconds")
                .LessThan(3 * 60 * 60).WithMessage("Duration cannot exceed 3 hours");
                
            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Release date cannot be in the future");
                
            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("Genre is required")
                .MaximumLength(50).WithMessage("Genre cannot exceed 50 characters");
                
            RuleFor(x => x.AlbumId)
                .GreaterThan(0).WithMessage("A valid album ID is required");
        }
    }
}