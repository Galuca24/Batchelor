using Licenta.Application.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Reviews
{
    public class ReviewDto
    {

        public UserReviewDto? CreatedBy { get; set; }
        public Guid BookId { get; set; }
        public Guid AudioBookId { get; set; }
        public Guid ReviewId { get; set; }
        public string ReviewText { get; set; }
        public DateTime DatePosted { get; set; }

        
    }
}
