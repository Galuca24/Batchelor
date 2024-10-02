using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.ResetCode.Commands
{
    public class CreateResetCodeCommandValidator : AbstractValidator<CreateResetCodeCommand>
    {
        public CreateResetCodeCommandValidator()
        {
            RuleFor(i => i.Email).NotEmpty().NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
        }
    }
}
