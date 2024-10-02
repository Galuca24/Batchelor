using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
     public interface IFineRepository : IAsyncRepository<Fine>
    {
        Task<Result<Fine>> UpdateFineStatus(Guid fineId);
        Task<Result<Fine>> CalculateAndSaveFine(int amount, string description,Guid userId,Guid bookId, int daysOverdue);
        Task<int> GetTotalFinesAmountAsync();
        Task<List<Fine>> GetUnpaidFinesByUserIdAsync(Guid userId);
        Task<int> GetTotalFinesAmountByUserAsync(Guid userId);

        Task<int> GetTotalUnpaidFinesAmountByUserIdAsync(Guid userId);

        Task<bool> HasUnpaidFinesAsync(Guid userId);

        Task<List<Fine>> GetFinesByUserIdAsync(Guid userId);

    }
}
