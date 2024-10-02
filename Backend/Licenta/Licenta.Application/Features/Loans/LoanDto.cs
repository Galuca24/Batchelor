using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans
{
    public class LoanDto
    {
        public Guid LoanId { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public string LoanedBy { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }

}
