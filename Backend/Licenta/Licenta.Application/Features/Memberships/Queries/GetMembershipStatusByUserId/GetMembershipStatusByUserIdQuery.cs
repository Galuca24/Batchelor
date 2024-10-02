using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Memberships.Queries.GetMembershipStatusByUserId
{
    public class GetMembershipStatusByUserIdQuery : IRequest<MembershipStatus>
    {
        public Guid UserId { get; set; }
    }
}
