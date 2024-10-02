using Licenta.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Domain.Entities
{
    public enum MembershipStatus
    {
        Active = 1,
        Expired = 2
    }

    public class Membership
    {
        public Guid MembershipId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Price { get; set; }
        public MembershipStatus MembershipStatus { get; set; }

        public User User { get; set; }

        private Membership()
        {

        }

        public Membership(Guid membershipId, Guid userId, DateTime startDate, DateTime endDate, int price, MembershipStatus membershipStatus)
        {
            MembershipId = membershipId;
            UserId = userId;
            StartDate = startDate;
            EndDate = endDate;
            Price = price;
            MembershipStatus = membershipStatus;
        }

       public static Result<Membership> Create(Guid userId, DateTime startDate, DateTime endDate, int price, MembershipStatus membershipStatus)
        {
            if (userId == Guid.Empty)
                return Result<Membership>.Failure("UserId cannot be empty.");

            

            if (price == 0)
                return Result<Membership>.Failure("Price cannot be 0.");

            var membership = new Membership(Guid.NewGuid(), userId, DateTime.UtcNow, endDate, price, membershipStatus);

            return Result<Membership>.Success(membership);
        }


    }
}
