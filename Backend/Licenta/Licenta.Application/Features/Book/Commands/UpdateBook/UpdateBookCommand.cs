using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.UpdateBook
{
    using MediatR;

    public class UpdateBookCommand : IRequest<BookDto>
    {
        public Guid BookId { get; set; }
        public UpdateBookDto Book { get; set; }
    }

}
