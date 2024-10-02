using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetTotalFinesAmountByUser
{
    public class GetTotalFinesAmountByUserQueryHandler : IRequestHandler<GetTotalFinesAmountByUserQuery, int>
    {
        private readonly IFineRepository _fineRepository;

        public GetTotalFinesAmountByUserQueryHandler(IFineRepository fineRepository)
        {
            _fineRepository = fineRepository;
        }

        public async Task<int> Handle(GetTotalFinesAmountByUserQuery request, CancellationToken cancellationToken)
        {
            return await _fineRepository.GetTotalFinesAmountByUserAsync(request.UserId);
        }
    }
}
