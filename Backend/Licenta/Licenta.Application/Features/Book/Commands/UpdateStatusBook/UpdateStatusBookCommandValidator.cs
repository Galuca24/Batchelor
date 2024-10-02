using FluentValidation;
using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.UpdateStatusBook
{
    public class UpdateStatusBookCommandValidator : AbstractValidator<UpdateStatusBookCommand>
    {
        public UpdateStatusBookCommandValidator()
        {
            RuleFor(p => p.BookId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(command => command.Status)
             .Must(status => status == BookStatus.Available || status == BookStatus.Loaned)
             .WithMessage("Statusul poate fi doar Available sau Loaned.");
        }
       
    }
}
