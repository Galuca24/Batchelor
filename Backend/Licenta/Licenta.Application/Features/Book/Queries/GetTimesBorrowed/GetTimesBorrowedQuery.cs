using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetTimesBorrowed
{
    public class GetTimesBorrowedQuery : IRequest<GetTimesBorrowedResponse>
    {
        public Guid BookId { get; set; }

        
    }
}
