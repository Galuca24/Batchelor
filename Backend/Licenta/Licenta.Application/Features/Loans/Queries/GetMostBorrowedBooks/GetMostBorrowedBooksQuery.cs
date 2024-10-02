using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetMostBorrowedBooks
{
    public class GetMostBorrowedBooksQuery : IRequest<List<MostBorrowedBookDto>>
    {
    }
}
