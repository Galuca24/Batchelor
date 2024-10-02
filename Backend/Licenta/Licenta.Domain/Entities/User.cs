using Licenta.Domain.Common;

namespace Licenta.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid UserId { get; set; }


        public List<Loan>? Loans { get; private set; }
       public List<Review>? Reviews { get; private set; }
        public List<Fine> Fines { get; private set; }

        private User(Guid userId)
        {
            UserId = userId;
          

            Loans = new List<Loan>();
            Reviews = new List<Review>();
            Fines = new List<Fine>();
        }

        public static Result<User> Create(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return Result<User>.Failure("UserId cannot be empty");
            }

            return Result<User>.Success(new User(userId));
        }

       

    }

   


}
