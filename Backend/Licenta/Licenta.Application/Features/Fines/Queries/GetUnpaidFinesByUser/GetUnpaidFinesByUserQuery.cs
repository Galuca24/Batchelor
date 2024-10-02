using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetUnpaidFinesByUser
{
    public class GetUnpaidFinesQuery : IRequest<List<UnpaidFineDto>>
    {
        public Guid UserId { get; set; }
    }
}
