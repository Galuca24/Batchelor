using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Command.AddUserPhoto
{
    public class AddUserPhotoCommandValidator : AbstractValidator<AddUserPhotoCommand>
    {
        public AddUserPhotoCommandValidator()
        {
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(p => p.PhotoUrl)
                .NotEmpty().WithMessage("PhotoUrl is required.");
        }
    }
}
