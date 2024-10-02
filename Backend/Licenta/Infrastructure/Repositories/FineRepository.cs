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
    public class FineRepository : BaseRepository<Fine>, IFineRepository
    {
        public FineRepository(LicentaContext context) : base(context)
        {

        }

        public async Task<Result<Fine>> UpdateFineStatus(Guid fineId)
        {
            var fine = await _context.Fines.FindAsync(fineId);

            if (fine == null)
            {
                return Result<Fine>.Failure($"Fine with id {fineId} not found.");
            }

            fine.IsPaid = true;
            fine.PaidAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Result<Fine>.Success(fine);
        }

        public async Task<List<Fine>> GetFinesByUserIdAsync(Guid userId)
        {
            return await _context.Fines
                                 .Where(f => f.UserId == userId).OrderByDescending(f => f.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<bool> HasUnpaidFinesAsync(Guid userId)
        {
            return await _context.Fines.AnyAsync(f => f.UserId == userId && !f.IsPaid);
        }

        public async Task<List<Fine>> GetUnpaidFinesByUserIdAsync(Guid userId)
        {
            return await _context.Fines
                                 .Where(f => f.UserId == userId && !f.IsPaid)
                                 .ToListAsync();
        }

        public async Task<int> GetTotalUnpaidFinesAmountByUserIdAsync(Guid userId)
        {
            return await _context.Fines
                                 .Where(f => f.UserId == userId && !f.IsPaid)
                                 .SumAsync(f => f.Amount);
        }

        public async Task<int> GetTotalFinesAmountByUserAsync(Guid userId)
        {
            return await _context.Fines.Where(f => f.UserId == userId).SumAsync(f => f.Amount);
        }

        public async Task<int> GetTotalFinesAmountAsync()
        {
            return await _context.Fines.SumAsync(f => f.Amount);
        }

        public async Task<Result<Fine>> CalculateAndSaveFine(int amount, string description,Guid userId,Guid bookId,int daysOverdue)
        {
            var fine = new Fine(amount, description,userId,false,bookId, daysOverdue, null);
            await _context.Fines.AddAsync(fine);
            await _context.SaveChangesAsync();

            return Result<Fine>.Success(fine);
        }
    }
}
