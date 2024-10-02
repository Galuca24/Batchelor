using Licenta.Application.Persistence;
using Licenta.Domain.Entities;

namespace Licenta.API.Services
{
    public class MembershipExpirationService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipExpirationService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public async Task CheckAndExpireMemberships()
        {
            var memberships = await _membershipRepository.GetAllAsync();

            foreach (var membership in memberships)
            {
                if (membership.EndDate <= DateTime.UtcNow && membership.MembershipStatus == MembershipStatus.Active)
                {
                    membership.MembershipStatus = MembershipStatus.Expired;
                    await _membershipRepository.UpdateAsync(membership);
                }
            }
        }
    }

}
