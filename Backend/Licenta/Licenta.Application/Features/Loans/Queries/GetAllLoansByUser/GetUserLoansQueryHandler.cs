using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetAllLoansByUser
{
    public class GetAllLoansByUserQueryHandler : IRequestHandler<GetUserLoansQuery, List<LoanDto>>
    {
        private readonly ILoanRepository _loanRepository;

        public GetAllLoansByUserQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<LoanDto>> Handle(GetUserLoansQuery request, CancellationToken cancellationToken)
        {
            var result = await _loanRepository.GetAllLoansByUserId(request.UserId);
            if (!result.IsSuccess)
                throw new Exception(result.Error); 

            return result.Value.Select(loan => new LoanDto
            {
                LoanId = loan.LoanId,
                BookId = loan.BookId,
                UserId = loan.UserId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate,
                LoanedBy = loan.LoanedBy
            }).ToList();
        }
    }
}
