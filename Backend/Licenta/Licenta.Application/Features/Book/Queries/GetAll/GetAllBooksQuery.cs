using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetAll
{
    public class GetAllBooksQuery : IRequest<List<BookDto>> { }

}
