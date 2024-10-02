using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Commands.RemoveBookFromFavourite
{
    public class RemoveBookFromFavouriteCommand : IRequest<RemoveBookFromFavouriteCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
       
    }
}
