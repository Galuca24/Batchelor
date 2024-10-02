using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Queries.SearchUsers
{
    public class SearchUsersQueryResponse : BaseResponse
    {
        public UserSearchDto[] Users { get; set; } = [];
    }
}
