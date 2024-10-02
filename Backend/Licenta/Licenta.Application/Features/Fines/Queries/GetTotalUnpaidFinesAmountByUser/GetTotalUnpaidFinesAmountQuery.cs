using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetTotalUnpaidFinesAmountByUser
{
    public class GetTotalUnpaidFinesAmountQuery : IRequest<int>
    {
        public Guid UserId { get; set; }
    }
}
