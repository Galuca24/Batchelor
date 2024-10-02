using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetRemainingTime
{
    public class GetRemainingTimeQueryResponse : BaseResponse
    {
        public TimeSpan RemainingTime { get; set; }
        public bool IsOverdue => RemainingTime.TotalSeconds < 0;
    }
}
