using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Ratings
{
    public class RatingDto
    {
        public Guid RatingId { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public double Value { get; set; }
    }
}
