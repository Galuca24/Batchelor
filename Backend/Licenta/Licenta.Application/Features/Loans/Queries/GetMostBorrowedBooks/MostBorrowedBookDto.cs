using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetMostBorrowedBooks
{
    public class MostBorrowedBookDto
    {
       // public Guid BookId { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }
        public int TimesBorrowed { get; set; }
    }
}
