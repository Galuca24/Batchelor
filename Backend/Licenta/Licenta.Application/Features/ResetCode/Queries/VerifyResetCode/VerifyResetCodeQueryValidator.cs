using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.ResetCode.Queries.VerifyResetCode
{
    public class VerifyResetCodeQueryValidator : AbstractValidator<VerifyResetCodeQuery>
    {
        public VerifyResetCodeQueryValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");
            RuleFor(p => p.Code)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
