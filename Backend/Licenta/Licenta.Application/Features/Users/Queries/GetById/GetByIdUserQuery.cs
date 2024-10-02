using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQuery : IRequest<GetByIdUserQueryResponse>
    {
        public string UserId { get; set; }
    }
}
