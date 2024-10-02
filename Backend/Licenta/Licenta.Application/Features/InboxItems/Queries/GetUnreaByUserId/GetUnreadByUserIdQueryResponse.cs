using Licenta.Application.Features.InboxItems.Queries.GetByUserId;
using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Queries.GetUnreaByUserId
{
    public class GetUnreadByUserIdQueryResponse : BaseResponse
    {
        public GetUnreadByUserIdQueryResponse() : base()
        {

        }
        public List<GetByUserIdDto> InboxItems { get; set; }
    }
}
