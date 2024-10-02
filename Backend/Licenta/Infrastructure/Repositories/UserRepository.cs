

using Licenta.Application.Persistence;
using Licenta.Domain.Entities;

namespace Infrastructure.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(LicentaContext context) : base(context)
        {
        }

       
    }
}
