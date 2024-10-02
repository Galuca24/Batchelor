using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites
{
    public class FavouriteDto
    {
        public Guid FavouriteId { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime AddedOn { get; set; }

    }
}
