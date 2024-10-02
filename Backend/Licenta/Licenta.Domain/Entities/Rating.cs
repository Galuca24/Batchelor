using Licenta.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Domain.Entities
{
    public class Rating
    {
        public Guid RatingId { get; set; }
        public double Value { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public Book Book { get; set; }

        public Rating()
        {
        }

        public Rating(double value, Guid bookId, Guid userId)
        {
            RatingId = Guid.NewGuid();
            Value = value;
            BookId = bookId;
            UserId = userId;
        }

        public static Result<Rating> Create (double value, Guid bookId, Guid userId)
        {
            if (value < 0 || value > 5)
            {
                return Result<Rating>.Failure("Rating value must be between 1 and 5");
            }

            if (bookId == Guid.Empty)
            {
                return Result<Rating>.Failure("BookId cannot be empty");
            }

            if (userId == Guid.Empty)
            {
                return Result<Rating>.Failure("UserId cannot be empty");
            }

            return Result<Rating>.Success(new Rating(value, bookId, userId));
        }
    }
}
