using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetFinesByUser
{
    public class GetFinesByUserQueryHandler : IRequestHandler<GetFinesByUserQuery, List<FinesDto>>
    {
        private readonly IFineRepository fineRepository;

        public GetFinesByUserQueryHandler(IFineRepository fineRepository)
        {
            this.fineRepository = fineRepository;
        }

        public async Task<List<FinesDto>> Handle(GetFinesByUserQuery request, CancellationToken cancellationToken)
        {
            var fines = await fineRepository.GetFinesByUserIdAsync(request.UserId);

            return fines.Select(fine => new FinesDto
            {
                FineId = fine.FineId,
                UserId = fine.UserId,
                Amount = fine.Amount,
                Description = fine.Description,
                CreatedAt = fine.CreatedAt,
                IsPaid = fine.IsPaid,
                PaidAt = fine.PaidAt,
                BookId = fine.BookId,
                DaysOverdue = fine.DaysOverdue
            }).ToList();
        }
    }
}
