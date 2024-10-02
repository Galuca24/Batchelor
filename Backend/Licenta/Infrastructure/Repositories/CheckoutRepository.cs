using Licenta.Application.Features.Checkouts;
using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
  public class CheckoutRepository : BaseRepository<Checkout>, ICheckoutRepository
{
    public CheckoutRepository(LicentaContext context) : base(context)
    {

    }


        public async Task<List<Checkout>> GetOverdueCheckoutsByUserIdAsync(Guid userId)
        {
            return await _context.Checkouts
                .Where(c => c.UserId == userId && c.Status == CheckoutStatus.Overdue)
                .ToListAsync();
        }



        public async Task<List<Checkout>> GetOverdueCheckoutsAsync()
        {
            return await _context.Checkouts
                .Where(c => c.Status == CheckoutStatus.Overdue)
                .ToListAsync();
        }


        public async Task<List<CheckoutDto>> GetRecentCheckoutsAsync()
        {
            return await _context.Checkouts
                .Where(c => c.ReturnDate != null)
                .OrderByDescending(c => c.ReturnDate)
                .Select(c => new CheckoutDto
                {
                    BookTitle = c.BookTitle,
                    Author = c.Author,
                    UserName = c.UserName,
                    UserId = c.user.UserId, 

                    IssueDate = c.IssueDate,
                    ReturnDate = c.ReturnDate
                })
                .ToListAsync();
        }


        public async Task<List<int>> GetReturnsInLastSevenDaysAsync()
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-6);
            var returnsByDay = await _context.Checkouts
                .Where(c => c.ReturnDate >= sevenDaysAgo)
                .GroupBy(c => c.ReturnDate.Value.Date)
                .Select(group => new { Day = group.Key, Count = group.Count() })
                .OrderBy(result => result.Day)
                .ToListAsync();

            // Asigură-te că pentru fiecare zi din ultimele șapte zile există o intrare
            var results = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                var day = sevenDaysAgo.AddDays(i).Date;
                var returnCount = returnsByDay.FirstOrDefault(ld => ld.Day == day)?.Count ?? 0;
                results.Add(returnCount);
            }

            return results;
        }


      

        public async Task<Result<Checkout>> FindActiveByBookId(Guid bookId)
        {
            var activeCheckout = await _context.Checkouts
                .Where(c => c.BookId == bookId && c.ReturnDate == null)
                .FirstOrDefaultAsync();

            if (activeCheckout == null)
                return Result<Checkout>.Failure("Nu există un împrumut activ pentru această carte.");

            return Result<Checkout>.Success(activeCheckout);
        }

        public async Task<int> CountReturnedBooksAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            IQueryable<Checkout> query = _context.Checkouts.Where(c => c.ReturnDate != null);

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(c => c.ReturnDate >= startDate.Value && c.ReturnDate <= endDate.Value);
            }

            return await query.CountAsync();
        }


        public async Task<int> CountReturnedBooksByUserAsync(Guid userId)
        {
            return await _context.Set<Checkout>()
                                 .CountAsync(c => c.UserId == userId && c.ReturnDate.HasValue);
        }
    }
}
