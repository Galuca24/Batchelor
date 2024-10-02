using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Checkouts.Queries.GetReturnsInTheLastSevenDays
{
    public class GetReturnsInLastSevenDaysQueryHandler : IRequestHandler<GetReturnsInLastSevenDaysQuery, List<int>>
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public GetReturnsInLastSevenDaysQueryHandler(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<List<int>> Handle(GetReturnsInLastSevenDaysQuery request, CancellationToken cancellationToken)
        {
            return await _checkoutRepository.GetReturnsInLastSevenDaysAsync();
        }
    }
}
