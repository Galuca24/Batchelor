using Licenta.Application.Features.Loans.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Queries.GetAll
{
    public class GetAllUsersResponse
    {
        public List<UserDto>? Users { get; set; }
    }
}
