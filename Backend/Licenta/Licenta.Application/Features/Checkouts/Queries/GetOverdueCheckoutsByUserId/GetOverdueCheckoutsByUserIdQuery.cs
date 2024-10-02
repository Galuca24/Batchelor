using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Checkouts.Queries.GetOverdueCheckoutsByUserId
{
    public class GetOverdueCheckoutsByUserIdQuery : IRequest<List<CheckoutDto>>
    {
        public Guid UserId { get; set; }
    }
}
