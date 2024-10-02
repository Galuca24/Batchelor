using Microsoft.EntityFrameworkCore;
using Licenta.Domain.Entities;

namespace Infrastructure
{
    public class LicentaContext : DbContext
    {
        public LicentaContext(DbContextOptions<LicentaContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        public DbSet<AudioBook> AudioBooks { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }
        public DbSet<UserPhoto> UserPhotos { get; set; }

        public DbSet<InboxItem> InboxItems { get; set; }

        public DbSet<Checkout> Checkouts { get; set; }

        public DbSet<Fine> Fines { get; set; }

        public DbSet<Loan> Loans { get; set; }
       
    }
}
