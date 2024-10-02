using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Fines
{
    public class FinesDto
    {
        public Guid FineId { get; set; }
        public Guid? UserId { get; set; }

        public Guid? BookId { get; set; }

        public int Amount { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public int DaysOverdue { get; set; }
        public bool IsPaid { get; set; }
    }
}
