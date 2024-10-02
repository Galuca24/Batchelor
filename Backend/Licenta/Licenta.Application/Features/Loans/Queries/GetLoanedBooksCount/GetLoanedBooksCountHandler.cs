using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetLoanedBooksCount
{
    public class GetLoanedBooksCountQueryHandler : IRequestHandler<GetLoanedBooksCountQuery, int>
    {
        private readonly IBookRepository _bookRepository;

        public GetLoanedBooksCountQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<int> Handle(GetLoanedBooksCountQuery request, CancellationToken cancellationToken)
        {
            return await _bookRepository.CountBooksByStatusAsync(BookStatus.Loaned);
        }
    }
}
