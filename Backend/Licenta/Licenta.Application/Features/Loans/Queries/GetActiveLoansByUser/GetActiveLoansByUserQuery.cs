using Licenta.Application.Features.Book;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetActiveLoansByUser
{
    public class GetActiveLoansByUserQuery : IRequest<List<BookDto>>
    {
        public Guid UserId { get; set; }
    }
}
