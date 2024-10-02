using Licenta.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Domain.Entities
{
    public class Fine
    {
        public Guid FineId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BookId { get; set; }

        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }

        public bool IsPaid { get; set; }

        public int DaysOverdue { get; set; }


        public Fine() { }

        public Fine(int amount, string description,Guid userId, bool isPaid, Guid? bookId, int daysOverdue, DateTime? paidAt)
        {
            FineId = Guid.NewGuid();
            UserId = userId;
            Amount = amount;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            IsPaid = isPaid;
            BookId = bookId;
            DaysOverdue = daysOverdue;
            PaidAt = paidAt;
        }

        public Result<Fine> Create(int amount, string description,Guid userId,bool isPaid,Guid bookId,int daysOverdue)
        {
            if (amount <= 0)
            {
                return Result<Fine>.Failure("Suma amenzii trebuie să fie mai mare decât 0.");
            }

            if (string.IsNullOrEmpty(description))
            {
                return Result<Fine>.Failure("Descrierea amenzii este obligatorie.");
            }

            return Result<Fine>.Success(new Fine(amount, description,userId,isPaid,bookId, daysOverdue,null));
        }
    }
}
