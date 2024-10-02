using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetTotalUnpaidFinesAmountByUser
{
    public class GetTotalUnpaidFinesAmountQueryHandler : IRequestHandler<GetTotalUnpaidFinesAmountQuery, int>
    {
        private readonly IFineRepository _fineRepository;

        public GetTotalUnpaidFinesAmountQueryHandler(IFineRepository fineRepository)
        {
            _fineRepository = fineRepository;
        }

        public async Task<int> Handle(GetTotalUnpaidFinesAmountQuery request, CancellationToken cancellationToken)
        {
            return await _fineRepository.GetTotalUnpaidFinesAmountByUserIdAsync(request.UserId);
        }
    }
}
