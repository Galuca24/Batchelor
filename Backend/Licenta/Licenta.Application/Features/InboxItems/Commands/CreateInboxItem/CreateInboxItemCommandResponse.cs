using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Commands.CreateInboxItem
{
    public class CreateInboxItemCommandResponse : BaseResponse
    {
        public CreateInboxItemCommandResponse() : base()
        {
        }
        public CreateInboxItemDto InboxItem { get; set; }
    }
}
