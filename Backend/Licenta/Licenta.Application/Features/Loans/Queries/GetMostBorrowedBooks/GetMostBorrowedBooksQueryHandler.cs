using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetMostBorrowedBooks
{
    public class GetMostBorrowedBooksQueryHandler : IRequestHandler<GetMostBorrowedBooksQuery, List<MostBorrowedBookDto>>
    {
        private readonly ILoanRepository _loanRepository;

        public GetMostBorrowedBooksQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<MostBorrowedBookDto>> Handle(GetMostBorrowedBooksQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetMostBorrowedBooksAsync();
            return loans;
        }
    }
}
