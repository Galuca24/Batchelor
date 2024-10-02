using Licenta.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Queries.GetUnreaByUserId
{
    public class GetUnreadByUserIdQuery : IRequest<GetUnreadByUserIdQueryResponse>
    {
        public Guid UserId { get; set; }
    }
    
}
