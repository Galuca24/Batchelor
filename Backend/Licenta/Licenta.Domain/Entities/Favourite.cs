using Licenta.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Domain.Entities
{
    public class Favourite
    {
        [Key]
        public Guid FavoriteId { get; set; } 
        public Guid? UserId { get; set; } 
        public Guid? BookId { get; set; } 
        public DateTime AddedOn { get; set; } 
        public User? User { get; set; }
        public Book? Book { get; set; }
        public Favourite()
        {
        }

        public Favourite(Guid userId, Guid bookId)
        {
            FavoriteId = Guid.NewGuid();
            UserId = userId;
            BookId = bookId;
            AddedOn = DateTime.Now;
        }

       


        public static Result<Favourite> Create(Guid userId, Guid bookId)
        {
            if (userId == Guid.Empty)
            {
                return Result<Favourite>.Failure("UserId cannot be empty");
            }

            if (bookId == Guid.Empty)
            {
                return Result<Favourite>.Failure("BookId cannot be empty");
            }

            return Result<Favourite>.Success(new Favourite(userId, bookId));
        }

    }
}
