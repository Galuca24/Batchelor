using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetCurrentLoansInLastSevenDays
{
    public class GetCurrentLoansQuery : IRequest<List<int>>
    {
    }
}
