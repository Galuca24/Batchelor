using Licenta.Domain.Entities;

namespace Licenta.Application.Features.Book.Commands.CreateBook
{
    public class CreateBookDto
    {
        public Guid BookId { get; set; } 
        public string?  Title { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        public BookStatus BookStatus { get; set; }
        
    }
}
