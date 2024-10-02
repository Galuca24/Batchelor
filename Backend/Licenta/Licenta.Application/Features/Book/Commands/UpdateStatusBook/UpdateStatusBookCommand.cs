using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.UpdateStatusBook
{
    public class UpdateStatusBookCommand : IRequest<UpdateStatusBookCommandResponse>
    {
        public Guid BookId { get; set; }
        public BookStatus Status { get; set; }
    }
   
}
