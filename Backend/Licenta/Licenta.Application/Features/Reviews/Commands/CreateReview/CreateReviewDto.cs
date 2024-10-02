using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewDto
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }

        public string ReviewText { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

    }
}
