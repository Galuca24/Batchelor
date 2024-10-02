using Licenta.Domain.Entities;
using MediatR;

namespace Licenta.Application.Features.Book.Commands.CreateBook
{
    public class CreateBookCommand: IRequest<CreateBookCommandResponse>
    {
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public BookStatus BookStatus { get; set; }

        public string? ImageUrl { get; set; }
        public int  NumberOfCopies { get; set; }

    }
   
}
