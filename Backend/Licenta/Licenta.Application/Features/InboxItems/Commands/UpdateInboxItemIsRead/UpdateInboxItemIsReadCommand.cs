using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead
{
    public class UpdateInboxItemIsReadCommand : IRequest<UpdateInboxItemIsReadCommandResponse>
    {
        public Guid InboxItemId { get; set; }
    }
}
