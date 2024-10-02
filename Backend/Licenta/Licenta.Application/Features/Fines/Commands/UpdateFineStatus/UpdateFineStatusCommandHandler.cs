using Licenta.Application.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Commands.UpdateFineStatus
{
    public class UpdateFineStatusCommandHandler : IRequestHandler<UpdateFineStatusCommand, UpdateFineStatusResponse>
    {
        private readonly IFineRepository _fineRepository;

        public UpdateFineStatusCommandHandler(IFineRepository fineRepository)
        {
            _fineRepository = fineRepository;
        }

        public async Task<UpdateFineStatusResponse> Handle(UpdateFineStatusCommand request, CancellationToken cancellationToken)
        {
            var fineResult = await _fineRepository.FindByIdAsync(request.FineId);

            if (!fineResult.IsSuccess || fineResult.Value == null) 
            {
                return new UpdateFineStatusResponse { Success = false, Message = "Fine not found." };
            }

            var fine = fineResult.Value; 

            await _fineRepository.UpdateFineStatus(fine.FineId);

            return new UpdateFineStatusResponse { Success = true, Message = "Fine status updated successfully." };
        }
    }

}
