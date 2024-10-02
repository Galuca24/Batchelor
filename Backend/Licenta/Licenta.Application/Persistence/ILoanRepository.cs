using Licenta.Application.Features.Book.Queries.GetMostBorrowedBooksInLast7Days;
using Licenta.Application.Features.Loans.Queries.GetMostBorrowedBooks;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;

namespace Licenta.Application.Persistence
{
    public interface ILoanRepository: IAsyncRepository<Loan>
    {
        Task<Result<Loan>> CreateLoanAsync(Guid bookId, Guid userId, DateTime dueDate);
        Task<Result<Loan>> GetActiveLoanByBookId(Guid bookId);
        Task<Result<List<Loan>>> GetActiveLoansByUserId(Guid userId);
        Task<Result<List<Loan>>> GetAllLoansByUserId(Guid userId);
        Task<IEnumerable<Loan>> GetOverdueLoansAsync();
        Task<IEnumerable<Loan>> GetReturnedOverdueLoansAsync();
        Task<List<int>> GetCurrentLoansAsync();
        Task<List<MostBorrowedBookDto>> GetMostBorrowedBooksAsync();
        Task<List<MostBorrowedBooksInLast7DaysDto>> GetMostBorrowedInLast7DaysBooksAsync();
        Task<int> CountActiveLoansByUserIdAsync(Guid userId);
        Task<int> GetLoanCountByBookId(Guid bookId);



    }
}
