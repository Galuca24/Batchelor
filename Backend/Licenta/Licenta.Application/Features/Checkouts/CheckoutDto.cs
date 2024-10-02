using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Checkouts
{
    public class CheckoutDto
    {
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string UserName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int DaysOverdue { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public int Fine { get; set; }

    }

}
