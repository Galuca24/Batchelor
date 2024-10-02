using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Checkouts.Queries.GetOverdueCheckouts
{
    public class GetOverdueCheckoutsHandler : IRequestHandler<GetOverdueCheckoutsQuery, List<CheckoutDto>>
    {

        private readonly ICheckoutRepository _checkoutRepository;

        public GetOverdueCheckoutsHandler(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<List<CheckoutDto>> Handle(GetOverdueCheckoutsQuery request, CancellationToken cancellationToken)
        {
            var overdueCheckouts = await _checkoutRepository.GetOverdueCheckoutsAsync();
            return overdueCheckouts.Select(c => new CheckoutDto
            {
                BookTitle = c.BookTitle,
                Author = c.Author,
                UserName = c.UserName,
                UserId = c.UserId,
                IssueDate = c.IssueDate,
                ReturnDate = c.ReturnDate,
                Fine = c.Fine,
                DaysOverdue = c.DaysOverdue,
                BookId = c.BookId
            }).ToList();
        }
    }
}
