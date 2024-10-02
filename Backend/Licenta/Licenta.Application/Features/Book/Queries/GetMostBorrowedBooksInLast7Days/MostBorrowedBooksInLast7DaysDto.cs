using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetMostBorrowedBooksInLast7Days
{
    public class MostBorrowedBooksInLast7DaysDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string ImageUrl { get; set; }
        public int BorrowedTimes { get; set; }
    }
}
