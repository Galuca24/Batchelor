using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Checkouts.Queries.GetOverdueCheckouts
{
    public class GetOverdueCheckoutsQuery : IRequest<List<CheckoutDto>>
    {
    }
}
