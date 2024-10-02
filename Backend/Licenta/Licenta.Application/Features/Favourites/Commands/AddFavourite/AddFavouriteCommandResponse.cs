using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Commands.AddFavourite
{
    public class AddFavouriteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
