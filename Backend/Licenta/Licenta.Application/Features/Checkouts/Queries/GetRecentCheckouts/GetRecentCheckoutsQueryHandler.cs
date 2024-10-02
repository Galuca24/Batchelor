using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Checkouts.Queries.GetRecentCheckouts
{
    public class GetRecentCheckoutsQueryHandler : IRequestHandler<GetRecentCheckoutsQuery, List<CheckoutDto>>
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public GetRecentCheckoutsQueryHandler(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<List<CheckoutDto>> Handle(GetRecentCheckoutsQuery request, CancellationToken cancellationToken)
        {
            return await _checkoutRepository.GetRecentCheckoutsAsync();
        }
    }
}
