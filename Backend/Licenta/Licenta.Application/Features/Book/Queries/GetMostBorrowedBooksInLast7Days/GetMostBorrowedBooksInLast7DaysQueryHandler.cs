using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetMostBorrowedBooksInLast7Days
{
    public class GetMostBorrowedBooksInLast7DaysQueryHandler : IRequestHandler<GetMostBorrowedBooksInLast7DaysQuery, List<MostBorrowedBooksInLast7DaysDto>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;

        public GetMostBorrowedBooksInLast7DaysQueryHandler(IBookRepository bookRepository, ILoanRepository loanRepository)
        {
            _bookRepository = bookRepository;
            _loanRepository = loanRepository;
        }

        public async Task<List<MostBorrowedBooksInLast7DaysDto>> Handle(GetMostBorrowedBooksInLast7DaysQuery request, CancellationToken cancellationToken)
        {
            // Aici putem folosi metoda deja definită în ILoanRepository, presupunând că implementează logica necesară
            var mostBorrowedBooks = await _loanRepository.GetMostBorrowedInLast7DaysBooksAsync();

            // Opțional, putem îmbogăți datele cu detalii suplimentare despre cărți dacă este necesar
            var bookIds = mostBorrowedBooks.Select(b => b.BookId).ToList();
            var books = await _bookRepository.FindByIdsAsync(bookIds);
            var bookDictionary = books.Value.ToDictionary(b => b.BookId, b => b);

            foreach (var book in mostBorrowedBooks)
            {
                book.Title = bookDictionary[book.BookId].Title;
            }

            return mostBorrowedBooks;
        }

    }
}
