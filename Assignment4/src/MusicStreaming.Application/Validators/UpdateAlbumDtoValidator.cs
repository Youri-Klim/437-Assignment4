using FluentValidation;
using MusicStreaming.Application.DTOs;
using System;

namespace MusicStreaming.Application.Validators
{
    public class UpdateAlbumDtoValidator : AbstractValidator<UpdateAlbumDto>
    {
        public UpdateAlbumDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid album ID is required");
                
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
                
            RuleFor(x => x.ArtistId)
                .GreaterThan(0).WithMessage("A valid artist ID is required");
        }
    }
}