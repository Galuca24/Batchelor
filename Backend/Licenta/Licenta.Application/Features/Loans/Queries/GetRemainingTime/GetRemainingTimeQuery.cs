using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetRemainingTime
{
    public class GetRemainingTimeQuery : IRequest<GetRemainingTimeQueryResponse>
    {
        public Guid BookId { get; set; }
    }
}
