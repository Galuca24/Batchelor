using Licenta.Application.Responses;

namespace Licenta.Application.Features.Book.Commands.CreateBook
{
    public class CreateBookCommandResponse : BaseResponse
    {
        public CreateBookCommandResponse() : base()
        {
        }

        public CreateBookDto Book { get; set; }
       
    }
}
