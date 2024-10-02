using Licenta.Application.Features.Book.Queries.GetMostBorrowedBooksInLast7Days;
using Licenta.Application.Features.Loans.Queries.GetMostBorrowedBooks;
using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class LoanRepository : BaseRepository<Loan>, ILoanRepository
    {
        public LoanRepository(LicentaContext context) : base(context)
        {

        }
        public async Task<int> GetLoanCountByBookId(Guid bookId)
        {
            return await _context.Loans.CountAsync(loan => loan.BookId == bookId);
        }

        public async Task<int> CountActiveLoansByUserIdAsync(Guid userId)
        {
            return await _context.Loans.CountAsync(l => l.UserId == userId && l.ReturnDate == null);
        }


        public async Task<List<MostBorrowedBooksInLast7DaysDto>> GetMostBorrowedInLast7DaysBooksAsync()
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            return await _context.Loans
                .Where(l => l.LoanDate >= sevenDaysAgo ) // Include doar împrumuturile active
                .GroupBy(l => l.BookId)
                .Select(group => new MostBorrowedBooksInLast7DaysDto
                {
                    BookId = group.Key,
                    Title = group.First().Book.Title,
                    Author = group.First().Book.Author,
                    Genre = group.First().Book.Genre,
                    Description = group.First().Book.Description,
                    ISBN = group.First().Book.ISBN,
                    ImageUrl = group.First().Book.ImageUrl,
                    BorrowedTimes = group.Count()
                })
                .OrderByDescending(dto => dto.BorrowedTimes)
                .ToListAsync();
        }


        public async Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync()
        {
            return await _context.Loans
                .Include(l => l.Book) // Include details about books
                .GroupBy(l => new { l.Book.Title, l.Book.Author }) // Group by both title and author
                .Select(group => new MostBorrowedBookDto
                {
                    Title = group.Key.Title,
                    Author = group.Key.Author,
                    TimesBorrowed = group.Count(),
                    // The BookId field might be tricky if you have multiple IDs for the same title and author
                    // You might need to decide if you want to return a BookId here
                    // BookId = group.First().BookId // Example if you want to keep BookId for reference
                })
                .OrderByDescending(b => b.TimesBorrowed)
                .ToListAsync();
        }


        public async Task<List<int>> GetCurrentLoansAsync()
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-6);
            var loansByDay = await _context.Loans
                .Where(l => l.LoanDate >= sevenDaysAgo )
                .GroupBy(l => l.LoanDate.Date)
                .Select(group => new { Day = group.Key, Count = group.Count() })
                .OrderBy(result => result.Day)
                .ToListAsync();

            // Asigură-te că pentru fiecare zi din ultimele șapte zile există o intrare
            var results = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                var day = sevenDaysAgo.AddDays(i).Date;
                var loanCount = loansByDay.FirstOrDefault(ld => ld.Day == day)?.Count ?? 0;
                results.Add(loanCount);
            }

            return results;
        }

        public async Task<IEnumerable<Loan>> GetReturnedOverdueLoansAsync()
        {
            return await _context.Loans
                .Where(loan => loan.ReturnDate != null && loan.ReturnDate > loan.DueDate)
                .Include(loan => loan.Book)
                .ToListAsync();
        }
        public async Task<IEnumerable<Loan>> GetOverdueLoansAsync()
        {
            return await _context.Loans
                .Where(loan => loan.ReturnDate == null && loan.DueDate < DateTime.UtcNow)
                .Include(loan => loan.Book)
                .ToListAsync();
        }

        public async Task<Result<List<Loan>>> GetAllLoansByUserId(Guid userId)
        {
            var loans = await _context.Loans
                                      .Where(l => l.UserId == userId)
                                      .OrderByDescending(l => l.LoanDate)  // Sort by most recent loan date
                                      .ToListAsync();

            if (loans == null || !loans.Any())
                return Result<List<Loan>>.Failure("No loans found for the specified user.");

            return Result<List<Loan>>.Success(loans);
        }


        public async Task<Result<List<Loan>>> GetActiveLoansByUserId(Guid userId)
        {
            try
            {
                var activeLoans = await _context.Loans
                    .Where(l => l.UserId == userId && l.ReturnDate == null)
                    .ToListAsync();
                return Result<List<Loan>>.Success(activeLoans);
            }
            catch (Exception ex)
            {
                return Result<List<Loan>>.Failure($"Error retrieving active loans for user {userId}: {ex.Message}");
            }
        }

        public async Task<Result<Loan>> GetActiveLoanByBookId(Guid bookId)
        {
            var loan = await _context.Loans
                .Where(l => l.BookId == bookId && l.ReturnDate == null)
                .FirstOrDefaultAsync();

            if (loan == null)
                return Result<Loan>.Failure("Nu există un împrumut activ pentru această carte.");

            return Result<Loan>.Success(loan);
        }

        public async Task<Result<Loan>> CreateLoanAsync(Guid bookId, Guid userId, DateTime dueDate)
        {
           

            var book = await _context.Books.FindAsync(bookId);
            if (book == null || book.BookStatus == BookStatus.Loaned)
            {
                return Result<Loan>.Failure("Book is not available.");
            }
           

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return Result<Loan>.Failure("User does not exist.");
            }

            var loan = new Loan
            {
                BookId = bookId,
                UserId = userId,
                LoanDate = DateTime.UtcNow,
                DueDate = dueDate
            };
            book.BookStatus = BookStatus.Loaned;

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            return Result<Loan>.Success(loan);
        }
    }
    
}
