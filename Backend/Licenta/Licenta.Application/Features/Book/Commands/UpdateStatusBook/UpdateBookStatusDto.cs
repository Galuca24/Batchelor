using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.UpdateStatusBook
{
    public class UpdateBookStatusDto
    {
        public BookStatus BookStatus { get; set; }
    }
}
