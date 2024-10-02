using Licenta.Application.Features.Book;
using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetActiveLoansByUser
{
    public class GetActiveLoansByUserQueryHandler : IRequestHandler<GetActiveLoansByUserQuery, List<BookDto>>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;

        public GetActiveLoansByUserQueryHandler(ILoanRepository loanRepository, IBookRepository bookRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
        }

        public async Task<List<BookDto>> Handle(GetActiveLoansByUserQuery request, CancellationToken cancellationToken)
        {
            var activeLoans = await _loanRepository.GetActiveLoansByUserId(request.UserId);
            var bookIds = activeLoans.Value.Select(l => l.BookId).Distinct().ToList();
            var books = await _bookRepository.FindByIdsAsync(bookIds);

            return books.Value
                .Select(b => new BookDto
                {
                    BookId = b.BookId,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    ISBN = b.ISBN,
                    Description = b.Description,
                    BookStatus = b.BookStatus
                })
                .ToList();
        }
    }
}
