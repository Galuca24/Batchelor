using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Commands.CreateInboxItem
{
    public class CreateInboxItemCommand : IRequest<CreateInboxItemCommandResponse>
    {
        public Guid UserId { get; set; }
        public string Message { get; set; } = default!;
    }
}
