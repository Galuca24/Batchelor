using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Queries.GetByUserId
{
    public class GetByUserIdQuery : IRequest<GetByUserIdQueryResponse>
    {
        public Guid UserId { get; set; }
    }
}
