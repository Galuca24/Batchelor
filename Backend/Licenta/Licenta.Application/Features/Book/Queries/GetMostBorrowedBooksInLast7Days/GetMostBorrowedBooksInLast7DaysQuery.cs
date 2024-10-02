using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetMostBorrowedBooksInLast7Days
{
    public class GetMostBorrowedBooksInLast7DaysQuery : IRequest<List<MostBorrowedBooksInLast7DaysDto>>
    {
    }
}
