using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface IMembershipRepository : IAsyncRepository<Membership>
    {

        Task<Membership> GetByUserIdAsync(Guid userId);
        Task<List<Membership>> GetAllAsync();


    }
}
