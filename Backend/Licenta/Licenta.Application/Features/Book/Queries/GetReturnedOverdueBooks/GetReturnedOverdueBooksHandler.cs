using Licenta.Application.Features.Book.Queries.GetReturnedOverdue.Licenta.Application.Features.Loans.Queries.GetOverdueBooks;
using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetReturnedOverdueBooks
{
    public class GetOverdueBooksQueryHandler : IRequestHandler<GetReturnedOverdueBooksQuery, List<OverdueBookDto>>
    {
        private readonly ILoanRepository _loanRepository;

        public GetOverdueBooksQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<OverdueBookDto>> Handle(GetReturnedOverdueBooksQuery request, CancellationToken cancellationToken)
        {
            var loans = await _loanRepository.GetReturnedOverdueLoansAsync();
            return loans.Select(loan => new OverdueBookDto
            {
                BookId = loan.BookId,
                Title = loan.Book.Title,
                DueDate = loan.DueDate,
                ReturnDate = loan.ReturnDate.Value
            }).ToList();
        }
    }
}
