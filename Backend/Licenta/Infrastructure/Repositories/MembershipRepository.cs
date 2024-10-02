using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MembershipRepository : BaseRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository(LicentaContext context) : base(context)
        {
        }

        public async Task<Membership> GetByUserIdAsync(Guid userId)
        {
            return await _context.Memberships
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.EndDate) // Dacă vrei să iei cel mai recent membership, ordonează descrescător după EndDate
                .FirstOrDefaultAsync();
        }

        public async Task<List<Membership>> GetAllAsync()
        {
            return await _context.Memberships.ToListAsync();
        }

    }
    
}
