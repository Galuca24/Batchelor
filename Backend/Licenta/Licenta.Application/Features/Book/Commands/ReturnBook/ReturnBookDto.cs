using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.ReturnBook
{
    public class ReturnBookDto
    {
        public Guid BookId { get; set; }

        public Guid UserId { get; set; }
    }
}
