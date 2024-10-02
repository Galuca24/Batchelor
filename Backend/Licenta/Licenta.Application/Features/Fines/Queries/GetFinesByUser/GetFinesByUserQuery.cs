using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines.Queries.GetFinesByUser
{
    public class GetFinesByUserQuery : IRequest<List<FinesDto>>
    {
        public Guid UserId { get; set; }
    }
}
