using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetReturnedOverdue
{
    namespace Licenta.Application.Features.Loans.Queries.GetOverdueBooks
    {
        public class GetReturnedOverdueBooksQuery : IRequest<List<OverdueBookDto>>
        {
        }
    }
}
