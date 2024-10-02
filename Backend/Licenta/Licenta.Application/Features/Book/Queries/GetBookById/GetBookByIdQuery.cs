using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<BookDto>
    {
        public Guid BookId { get; set; }
    }
}
