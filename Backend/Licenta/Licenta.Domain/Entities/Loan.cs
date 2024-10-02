using Licenta.Domain.Common;

namespace Licenta.Domain.Entities
{
    public class Loan 
    {
        public Guid LoanId { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? LoanedBy { get; set; }

        public int DaysLoaned { get; set;}

        public Book Book { get; set; }
        public User User { get; set; }

        private Loan(Guid loanId, Guid bookId, DateTime loanDate, DateTime? returnDate, Guid userId ,DateTime dueDate,string loanedBy)
        {
            LoanId = loanId;
            BookId = bookId;
            LoanDate = loanDate;
            ReturnDate = returnDate;
            DueDate = dueDate;
            UserId = userId;
            LoanedBy = loanedBy;
        }
        public Loan() { }


        public static Result<Loan> Create(Guid bookId, Guid userId, DateTime dueDate,string? loanedBy)
        {
            if (bookId == Guid.Empty)
                return Result<Loan>.Failure("BookId cannot be empty.");

            if (userId == Guid.Empty)
                return Result<Loan>.Failure("UserId cannot be empty.");
            if(string.IsNullOrEmpty(loanedBy))
                return Result<Loan>.Failure("Loaner cannot be empty.");



            var loan = new Loan(Guid.NewGuid(), bookId, DateTime.UtcNow, null, userId, dueDate, loanedBy);

            return Result<Loan>.Success(loan);
        }

    }
}
