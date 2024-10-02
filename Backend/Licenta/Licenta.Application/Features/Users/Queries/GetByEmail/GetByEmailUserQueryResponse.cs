using Licenta.Application.Features.Loans.Queries;
using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Queries.GetByEmail
{
    public class GetByEmailUserQueryReponse : BaseResponse
    {
        public GetByEmailUserQueryReponse() : base()
        {

        }
        public UserDto User { get; set; }
    }
}
