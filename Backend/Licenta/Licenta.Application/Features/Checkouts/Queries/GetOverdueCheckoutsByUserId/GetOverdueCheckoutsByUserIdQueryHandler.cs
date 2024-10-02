using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Checkouts.Queries.GetOverdueCheckoutsByUserId
{
    public class GetOverdueCheckoutsByUserIdQueryHandler : IRequestHandler<GetOverdueCheckoutsByUserIdQuery, List<CheckoutDto>>
    {
        private readonly ICheckoutRepository _checkoutRepository;

        public GetOverdueCheckoutsByUserIdQueryHandler(ICheckoutRepository checkoutRepository)
        {
            _checkoutRepository = checkoutRepository;
        }

        public async Task<List<CheckoutDto>> Handle(GetOverdueCheckoutsByUserIdQuery request, System.Threading.CancellationToken cancellationToken)
        {
            var overdueCheckouts = await _checkoutRepository.GetOverdueCheckoutsByUserIdAsync(request.UserId);
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

