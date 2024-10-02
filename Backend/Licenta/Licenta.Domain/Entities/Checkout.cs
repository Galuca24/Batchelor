using Licenta.Domain.Common;
using System;

namespace Licenta.Domain.Entities
{
    public class Checkout 
    {
        public Guid CheckoutId { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public CheckoutStatus Status { get; set; }

        public int DaysOverdue { get; set;}
        public int Fine { get; set; } = 0;

        public Book Book { get; set; }
        public User user { get; set; }

        public Checkout(Guid bookId, Guid userId, DateTime issueDate, DateTime dueDate,DateTime returnDate,string bookTitle,string author,string userName, int daysOverdue)
        {
            CheckoutId = Guid.NewGuid();
            BookId = bookId;
            UserId = userId;
            IssueDate = issueDate;
            DueDate = dueDate;
            Status = CheckoutStatus.Loaned; 
            ReturnDate = returnDate;
            BookTitle = bookTitle;
            Author = author;
            UserName = userName;
            DaysOverdue = daysOverdue;
        }

        public Checkout()
        {
        }
        
        public static Result<Checkout> Create(Guid bookId, Guid memberId, DateTime issueDate, DateTime dueDate, DateTime returnDate,string bookTitle,string author,string userName,int daysOverdue)
        {
            if (issueDate >= dueDate)
            {
                return Result<Checkout>.Failure("Data de returnare trebuie să fie după data de împrumut.");
            }

            
            return Result<Checkout>.Success(new Checkout(bookId, memberId, issueDate, dueDate, returnDate,bookTitle,author,userName,daysOverdue));
        }

   
        public void RegisterReturn()
        {
            ReturnDate = DateTime.UtcNow; 
            Status = CheckoutStatus.Returned;
            Book.BookStatus = BookStatus.Available;
        }

        public void CalculateFine()
        {
            if (ReturnDate.HasValue && ReturnDate.Value > DueDate)
            {
                Fine = (ReturnDate.Value - DueDate).Days * 1; 
            }
        }
    }

    public enum CheckoutStatus
    {
        Loaned=1,   
        Returned=2, 
        Overdue=3   
        
    }
}
