using Licenta.Application.Features.Loans.Queries;
using Licenta.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface IUserManager
    {
        Task<Result<UserDto>> UserExists(Guid userId);
        Task<Result<UserDto>> FindByIdAsync(Guid userId);
        Task<Result<UserDto>> FindByEmailAsync(string email);
        Task<Result<UserDto>> FindByUsernameAsync(string username);

        Task<Result<List<UserDto>>> FindByRole(string role);

        Task<Result<List<UserDto>>> GetAllAsync();
        Task<Result<UserDto>> DeleteAsync(Guid userId);
        Task<Result<UserDto>> UpdateAsync(UserDto user);
        Task<Result<UserDto>> UpdateRoleAsync(UserDto user, string role);

    }
}
