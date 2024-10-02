using Licenta.Domain.Common;

namespace Licenta.Domain.Entities
{
    public enum BookStatus
    {
        Available=1,
        Loaned=2,
        Reserved=3,
        
    }
    public class Book
    {
        public Guid BookId { get; set; }
        public string? Title { get; set; }
        public string ISBN { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int NumberOfCopies { get; set; }

        public BookStatus BookStatus { get; set; }

        public List<Loan>? Loans { get; private set; } 
        public List<Review>? Reviews { get; private set; } 
        public List<Checkout> Checkouts { get; private set; }
        public List<Rating> Ratings { get; private set; }

        private Book()
        {
            
        }
        public Book(Guid bookId, string title, string author, string genre,string description,string isbn,BookStatus bookStatus,string imageUrl,int numberOfCopies)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
            ISBN = isbn;
            BookStatus = bookStatus;
            ImageUrl = imageUrl;
            NumberOfCopies = numberOfCopies;
        
            Loans = new List<Loan>();
            Reviews = new List<Review>();
            Checkouts = new List<Checkout>();
            Ratings = new List<Rating>();
        }

        public static Result<Book> Create(string title, string author, string genre, string description,string isbn,BookStatus bookStatus,string imageUrl,int numberOfCopies) 
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result<Book>.Failure("Titlul este obligatoriu.");
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                return Result<Book>.Failure("Autorul este obligatoriu.");
            }

            if (string.IsNullOrWhiteSpace(genre))
            {
                return Result<Book>.Failure("Genul este obligatoriu.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<Book>.Failure("Descrierea este obligatorie.");
            }
            if(string.IsNullOrWhiteSpace(isbn))
            {
                return Result<Book>.Failure("ISBN este obligatoriu.");
            }
            
            
            
            return Result<Book>.Success(new Book(Guid.NewGuid(), title, author, genre, description,isbn,bookStatus,imageUrl,numberOfCopies));

        }

        public void  RegisterReturn()
        {
            BookStatus = BookStatus.Available;
        }

       

    }
}
