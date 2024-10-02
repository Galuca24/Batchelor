using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Commands.LoanBookCommand
{
    public class LoanBookCommandResponse: BaseResponse
    {
        public LoanBookCommandResponse() : base()
        {
        }

        public LoanBookDto Loan { get; set; }
    }
    
}
