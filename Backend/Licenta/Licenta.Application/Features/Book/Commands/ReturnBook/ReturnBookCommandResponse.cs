using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.ReturnBook
{
    public class ReturnBookCommandResponse : BaseResponse
    {
        public ReturnBookCommandResponse() : base()
        {
        }

        public Guid? CheckoutId { get; set; }
    }
}
