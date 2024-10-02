using Licenta.Application.Responses;


namespace Licenta.Application.Features.Book.Commands.UpdateStatusBook
{
    public class UpdateStatusBookCommandResponse : BaseResponse
    {
        public UpdateStatusBookCommandResponse() : base()
        {
        }

        public UpdateBookStatusDto Book { get; set; }
    }
}
