using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Commands.CreateInboxItem
{
    public class CreateInboxItemCommandValidator : AbstractValidator<CreateInboxItemCommand>
    {
        public CreateInboxItemCommandValidator()
        {
            RuleFor(i => i.UserId).NotEmpty().NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
            RuleFor(i => i.Message).NotEmpty().WithMessage("{PropertyName} is required")
                .NotNull();
        }
    }
}
