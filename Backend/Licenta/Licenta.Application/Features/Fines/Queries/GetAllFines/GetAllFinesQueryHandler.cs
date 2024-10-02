using Licenta.Application.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetAllFines
{
    public class GetAllFinesQueryHandler : IRequestHandler<GetAllFinesQuery, List<FinesDto>>
    {
        private readonly IFineRepository _fineRepository;

        public GetAllFinesQueryHandler(IFineRepository fineRepository)
        {
            _fineRepository = fineRepository;
        }

        public async Task<List<FinesDto>> Handle(GetAllFinesQuery request, CancellationToken cancellationToken)
        {
            var result = await _fineRepository.GetAllAsync();

            // Check if the operation was successful
            if (!result.IsSuccess)
            {
                // Handle the error scenario, possibly by throwing an exception or returning an empty list
                throw new Exception("Failed to retrieve fines: " + result.Error);
            }

            // If successful, map the data to DTOs
            var fines = result.Value.Select(f => new FinesDto
            {
                FineId = f.FineId,
                BookId = f.BookId,
                DaysOverdue = f.DaysOverdue,
                UserId = f.UserId,
                Amount = f.Amount,
                Description = f.Description,
                CreatedAt = f.CreatedAt,
                PaidAt = f.PaidAt,
                IsPaid = f.IsPaid
            }).ToList();

            return fines;
        }
    }
}
