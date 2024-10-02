using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Queries.GetByUserId
{
    public class GetByUserIdQueryResponse : BaseResponse
    {
        public GetByUserIdQueryResponse() : base()
        {

        }
        public List<GetByUserIdDto> InboxItems { get; set; }
    }
}
