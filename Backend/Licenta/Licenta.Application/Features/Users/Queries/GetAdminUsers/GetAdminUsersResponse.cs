using Licenta.Application.Features.Loans.Queries;
using Licenta.Application.Responses;

namespace Licenta.Application.Features.Users.Queries.GetAdminUsers
{
    public class GetAdminUsersResponse 
    {
        

        public List<UserDto>? Users { get; set; }
        
    }
}