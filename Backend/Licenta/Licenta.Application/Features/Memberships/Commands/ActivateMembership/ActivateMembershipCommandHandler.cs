using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Memberships.Commands.ActivateMembership
{
    public class ActivateMembershipCommandHandler : IRequestHandler<ActivateMembershipCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMembershipRepository _membershipRepository;

        public ActivateMembershipCommandHandler(IUserRepository userRepository, IMembershipRepository membershipRepository)
        {
            _userRepository = userRepository;
            _membershipRepository = membershipRepository;
        }

        public async Task<string> Handle(ActivateMembershipCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return "User not found.";
            }

            var membership = await _membershipRepository.GetByUserIdAsync(request.UserId);
            if (membership != null)
            {
                if (membership.MembershipStatus == MembershipStatus.Expired)
                {
                    membership.MembershipStatus = MembershipStatus.Active;
                    membership.EndDate = DateTime.UtcNow.AddMonths(1);
                    await _membershipRepository.UpdateAsync(membership);
                }
                else
                {
                    return "Membership is already active.";
                }
            }
            else
            {
                var newMembershipResult = Membership.Create(request.UserId, DateTime.UtcNow, DateTime.UtcNow.AddMonths(1), 10, MembershipStatus.Active);
                if (!newMembershipResult.IsSuccess)
                {
                    return newMembershipResult.Error;
                }

                await _membershipRepository.AddAsync(newMembershipResult.Value);
            }

            return "Membership activated successfully.";
        }
    }
}
