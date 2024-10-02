using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Queries.GetByEmail
{
    public class GetByEmailUserQuery : IRequest<GetByEmailUserQueryReponse>
    {
        public string Email { get; set; }
    }
}
