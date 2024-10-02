using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetReturnedBooksCountByUser
{
    public class GetReturnedBooksCountByUserQueryHandler : IRequestHandler<GetReturnedBooksCountByUserQuery, int>
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public GetReturnedBooksCountByUserQueryHandler(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<int> Handle(GetReturnedBooksCountByUserQuery request, CancellationToken cancellationToken)
        {
            return await _checkoutRepository.CountReturnedBooksByUserAsync(request.UserId);
        }
    }
}
