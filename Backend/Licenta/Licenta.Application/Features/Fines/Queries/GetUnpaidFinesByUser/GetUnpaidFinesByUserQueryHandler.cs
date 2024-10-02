using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetUnpaidFinesByUser
{
    public class GetUnpaidFinesQueryHandler : IRequestHandler<GetUnpaidFinesQuery, List<UnpaidFineDto>>
    {
        private readonly IFineRepository _fineRepository;

        public GetUnpaidFinesQueryHandler(IFineRepository fineRepository)
        {
            _fineRepository = fineRepository;
        }

        public async Task<List<UnpaidFineDto>> Handle(GetUnpaidFinesQuery request, CancellationToken cancellationToken)
        {
            var fines = await _fineRepository.GetUnpaidFinesByUserIdAsync(request.UserId);
            return fines.Select(f => new UnpaidFineDto
            {
                FineId = f.FineId,
                Amount = f.Amount,
                Description = f.Description,
                CreatedAt = f.CreatedAt,
                IsPaid = f.IsPaid
            }).ToList();
        }
    }
}
