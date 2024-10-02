using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Memberships.Commands.ActivateMembership
{
    public class ActivateMembershipCommand : IRequest<string>
    {
        public Guid UserId { get; set; }

        public ActivateMembershipCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
