using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Command.UpdateUserPhoto
{
    public class UpdateUserPhotoCommandValidator : AbstractValidator<UpdateUserPhotoCommand>
    {
        public UpdateUserPhotoCommandValidator()
        {
            RuleFor(p => p.UserPhotoId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
            RuleFor(p => p.PhotoUrl)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}
