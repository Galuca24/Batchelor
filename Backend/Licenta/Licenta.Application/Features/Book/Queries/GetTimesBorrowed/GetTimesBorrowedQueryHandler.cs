using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetTimesBorrowed
{
    public class GetTimesBorrowedQueryHandler : IRequestHandler<GetTimesBorrowedQuery, GetTimesBorrowedResponse>
    {
        private readonly ILoanRepository _loanRepository;

        public GetTimesBorrowedQueryHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<GetTimesBorrowedResponse> Handle(GetTimesBorrowedQuery request, CancellationToken cancellationToken)
        {
            var loanCount = await _loanRepository.GetLoanCountByBookId(request.BookId);

            if (loanCount >= 0)
            {
                return new GetTimesBorrowedResponse
                {
                    TimesBorrowed = loanCount,
                    Success = true,
                    Message = "Retrieved successfully."
                };
            }

            return new GetTimesBorrowedResponse
            {
                TimesBorrowed = 0,
                Success = false,
                Message = "Book has not been borrowed or does not exist."
            };
        }
    }
}
