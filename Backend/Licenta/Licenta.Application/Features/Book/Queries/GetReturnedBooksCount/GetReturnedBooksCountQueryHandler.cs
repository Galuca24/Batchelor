using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetReturnedBooksCount
{
    public class GetReturnedBooksCountQueryHandler : IRequestHandler<GetReturnedBooksCountQuery, int>
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public GetReturnedBooksCountQueryHandler(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<int> Handle(GetReturnedBooksCountQuery request, CancellationToken cancellationToken)
        {
            return await _checkoutRepository.CountReturnedBooksAsync();
        }
    }
}
