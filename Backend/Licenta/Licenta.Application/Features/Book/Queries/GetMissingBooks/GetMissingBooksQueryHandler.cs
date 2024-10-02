using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetMissingBooks
{
    public class GetMissingBooksQueryHandler : IRequestHandler<GetMissingBooksQuery, List<MissingBookDto>>
    {
        private readonly ILoanRepository _loanRepository;

        public GetMissingBooksQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<MissingBookDto>> Handle(GetMissingBooksQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetOverdueLoansAsync();
            return loans.Select(loan => new MissingBookDto
            {
                BookId = loan.BookId,
                Title = loan.Book.Title,
                DueDate = loan.DueDate
            }).ToList();
        }
    }
}
