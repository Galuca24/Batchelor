using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Memberships.Queries.GetMembershipStatusByUserId
{
    public class GetMembershipStatusByUserIdQueryHandler : IRequestHandler<GetMembershipStatusByUserIdQuery, MembershipStatus>
    {
        private readonly IMembershipRepository _membershipRepository;

        public GetMembershipStatusByUserIdQueryHandler(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task<MembershipStatus> Handle(GetMembershipStatusByUserIdQuery request, System.Threading.CancellationToken cancellationToken)
        {
            var membership = await _membershipRepository.GetByUserIdAsync(request.UserId);

            if (membership == null)
                return MembershipStatus.Expired;

            if (membership.EndDate < DateTime.UtcNow)
                return MembershipStatus.Expired;

            return membership.MembershipStatus;
        }

    }
}
