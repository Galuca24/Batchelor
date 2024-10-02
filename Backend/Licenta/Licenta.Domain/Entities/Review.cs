using Licenta.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Domain.Entities
{
    public class Review
    {
        public Guid ReviewId { get; set; } 
        public Guid UserId { get; set; } 
        public Guid? BookId { get; set; } 
        public Guid? AudioBookId { get; set; }


        public string? ReviewText { get; set; } 
        public DateTime DatePosted { get; set; } 
    
        public User? User { get; set; } 
        public Book? Book { get; set; }
        public AudioBook? AudioBook { get; set; }
        public Review()
        {
        }

        private Review(Guid userId, Guid? bookId, Guid? audioBookId, string text, DateTime datePosted)
        {
            ReviewId = Guid.NewGuid();
            UserId = userId;
            BookId = bookId;
            AudioBookId = audioBookId;
            ReviewText = text;
            DatePosted = datePosted;
        }

        public static Result<Review> Create(Guid userId, Guid? bookId, Guid? audioBookId, string text, DateTime datePosted)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Result<Review>.Failure("Textul este obligatoriu.");
            }

            if (bookId == null && audioBookId == null)
            {
                return Result<Review>.Failure("Trebuie specificat un BookId sau un AudioBookId.");
            }

            return Result<Review>.Success(new Review(userId, bookId, audioBookId, text, datePosted));
        }
    }
}
