namespace Licenta.Application.Features.Book.Queries.GetReturnedOverdue.Licenta.Application.Features.Loans.Queries.GetOverdueBooks
{
    public class OverdueBookDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }

}