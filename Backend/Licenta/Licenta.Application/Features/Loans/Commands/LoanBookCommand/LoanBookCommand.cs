using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Commands.LoanBookCommand
{
    public class LoanBookCommand: IRequest<LoanBookCommandResponse>
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
    }
}
