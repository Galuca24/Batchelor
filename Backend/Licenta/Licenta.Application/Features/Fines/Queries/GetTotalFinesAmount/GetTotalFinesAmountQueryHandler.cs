using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetTotalFinesAmount
{
    public class GetTotalFinesAmountQueryHandler : IRequestHandler<GetTotalFinesAmountQuery, int>
    {
        private readonly IFineRepository _fineRepository;

        public GetTotalFinesAmountQueryHandler(IFineRepository fineRepository)
        {
            _fineRepository = fineRepository;
        }

        public async Task<int> Handle(GetTotalFinesAmountQuery request, CancellationToken cancellationToken)
        {
            return await _fineRepository.GetTotalFinesAmountAsync();
        }
    }
}
