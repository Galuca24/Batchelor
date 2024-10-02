using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetCurrentLoansInLastSevenDays
{
    public class GetCurrentLoansQueryHandler : IRequestHandler<GetCurrentLoansQuery, List<int>>
    {
        private readonly ILoanRepository _loanRepository;

        public GetCurrentLoansQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<int>> Handle(GetCurrentLoansQuery request, CancellationToken cancellationToken)
        {
            return await _loanRepository.GetCurrentLoansAsync();
        }
    }
}
