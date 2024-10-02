using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface IUserPhotoRepository : IAsyncRepository<UserPhoto>
    {
        Task<Result<UserPhoto>> GetUserPhotoByUserIdAsync(string userId);
    }
}
