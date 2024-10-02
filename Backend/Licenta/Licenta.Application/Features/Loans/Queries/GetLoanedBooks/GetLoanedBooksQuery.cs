using Licenta.Application.Features.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetLoanedBooks
{
    public class GetLoanedBooksQuery : IRequest<List<BookDto>>
    {
    }

}
