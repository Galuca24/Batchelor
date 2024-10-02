using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(p => p.ReviewText)
                .NotEmpty().WithMessage("{PropertyName} este obligatoriu.")
                .NotNull()
                .MaximumLength(500).WithMessage("{PropertyName} nu poate avea mai mult de 500 de caractere.");
        }
    }
}
