using Licenta.Domain.Entities;

namespace Licenta.Application.Persistence
{
    public interface IUserRepository: IAsyncRepository<User>
    {
    }
}
