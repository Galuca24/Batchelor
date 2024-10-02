using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Commands.UpdateFineStatus
{
    public class UpdateFineStatusCommand : IRequest<UpdateFineStatusResponse>
    {
        public Guid FineId { get; set; }
        
    }
}
