using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Licenta.Application.Features.Loans.Queries;
using Licenta.Application.Features.Users.Queries.GetAdminUsers;
using Licenta.Application.Features.Users.Queries.GetAll;
using Licenta.Application.Persistence;
using MediatR;

namespace Licenta.Application.Handlers.Users
{
    public class GetAdminUsersQueryHandler : IRequestHandler<GetAdminUsersQuery, GetAdminUsersResponse>
    {
        private readonly IUserManager _userManager;

        public GetAdminUsersQueryHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetAdminUsersResponse> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.FindByRole("Admin");
            var userDtos = new List<UserDto>();

            foreach (var user in users.Value)
            {
                userDtos.Add(new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Name = user.Name,
                    Email = user.Email,
                    Bio = user.Bio,
                    Mobile = user.Mobile,
                    Company = user.Company,
                    Location = user.Location,
                    Social = user.Social,
                    Roles = user.Roles.ToList()
                });
            }
            return new GetAdminUsersResponse { Users = userDtos };
        }
    }
}