using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetLoanedBooksCount
{
    public class GetLoanedBooksCountQuery : IRequest<int> 
    { 
    }
}
