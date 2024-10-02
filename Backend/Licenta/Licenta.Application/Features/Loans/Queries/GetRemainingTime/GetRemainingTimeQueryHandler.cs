using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetRemainingTime
{
    public class GetRemainingTimeQueryHandler : IRequestHandler<GetRemainingTimeQuery, GetRemainingTimeQueryResponse>
    {
        private readonly ILoanRepository _loanRepository;

        public GetRemainingTimeQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<GetRemainingTimeQueryResponse> Handle(GetRemainingTimeQuery request, CancellationToken cancellationToken)
        {
            var loanResult = await _loanRepository.GetActiveLoanByBookId(request.BookId);
            if (!loanResult.IsSuccess || loanResult.Value == null)
            {
                return new GetRemainingTimeQueryResponse { Success = false, Message = "Nu există un împrumut activ pentru această carte sau cartea nu a fost găsită." };
            }

            var loan = loanResult.Value;
            var remainingTime = loan.DueDate - DateTime.UtcNow;

            return new GetRemainingTimeQueryResponse
            {
                Success = true,
                RemainingTime = remainingTime,
                Message = "Timpul rămas a fost calculat cu succes."
            };
        }
    }
}
