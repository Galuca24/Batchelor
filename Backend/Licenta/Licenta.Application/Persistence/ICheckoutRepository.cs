using Licenta.Application.Features.Checkouts;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface ICheckoutRepository: IAsyncRepository<Checkout>
    {

        Task<List<Checkout>> GetOverdueCheckoutsByUserIdAsync(Guid userId);

        Task<Result<Checkout>> FindActiveByBookId(Guid bookId);
        Task<int> CountReturnedBooksByUserAsync(Guid userId);
        Task<int> CountReturnedBooksAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<List<int>> GetReturnsInLastSevenDaysAsync();

        Task<List<CheckoutDto>> GetRecentCheckoutsAsync();

        Task<List<Checkout>> GetOverdueCheckoutsAsync();

    }
}
