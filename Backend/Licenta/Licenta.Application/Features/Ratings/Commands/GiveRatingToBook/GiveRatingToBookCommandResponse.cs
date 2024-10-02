using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Ratings.Commands.GiveRatingToBook
{
    public class GiveRatingToBookCommandResponse : BaseResponse
    {
        public GiveRatingToBookCommandResponse() : base()
        {
        }

        public RatingDto Rating { get; set; }
       
    }
    
}
