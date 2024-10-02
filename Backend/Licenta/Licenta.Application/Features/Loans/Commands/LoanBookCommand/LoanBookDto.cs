using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Commands.LoanBookCommand
{
    public class LoanBookDto
    {
        Guid UserId { get; set; }
        Guid BookId { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }


    }
}
