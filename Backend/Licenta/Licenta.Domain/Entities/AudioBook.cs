using Licenta.Domain.Common;

namespace Licenta.Domain.Entities
{
    public class AudioBook
    {
        public Guid AudioBookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string AudioUrl { get; set; }
        public TimeSpan Duration { get; set; }
        public string ImageUrl { get; set; }

        public List<Loan>? Loans { get; private set; }
        public List<Review>? Reviews { get; private set; }
        public List<Rating>? Ratings { get; private set; }
        public List<Chapter>? Chapters { get; private set; }

        private AudioBook() { }

        public AudioBook(Guid audioBookId, string title, string author, string genre, string description, string isbn, string audioUrl, TimeSpan duration, string imageUrl)
        {
            AudioBookId = audioBookId;
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
            ISBN = isbn;
            AudioUrl = audioUrl;
            Duration = duration;
            ImageUrl = imageUrl;

            Loans = new List<Loan>();
            Reviews = new List<Review>();
            Ratings = new List<Rating>();
            Chapters = new List<Chapter>();
        }

        public static Result<AudioBook> Create(string title, string author, string genre, string description, string isbn, string audioUrl, TimeSpan duration, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return Result<AudioBook>.Failure("Titlul este obligatoriu.");
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                return Result<AudioBook>.Failure("Autorul este obligatoriu.");
            }

            if (string.IsNullOrWhiteSpace(genre))
            {
                return Result<AudioBook>.Failure("Genul este obligatoriu.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                return Result<AudioBook>.Failure("Descrierea este obligatorie.");
            }

            if (string.IsNullOrWhiteSpace(isbn))
            {
                return Result<AudioBook>.Failure("ISBN este obligatoriu.");
            }

            if (string.IsNullOrWhiteSpace(audioUrl))
            {
                return Result<AudioBook>.Failure("URL-ul audio este obligatoriu.");
            }

            return Result<AudioBook>.Success(new AudioBook(Guid.NewGuid(), title, author, genre, description, isbn, audioUrl, duration, imageUrl));
        }
    }
}
