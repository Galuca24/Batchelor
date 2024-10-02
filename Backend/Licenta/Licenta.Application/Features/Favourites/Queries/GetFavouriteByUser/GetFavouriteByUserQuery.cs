﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Queries.GetFavouriteByUser
{
    public class GetFavouriteByUserQuery : IRequest<List<FavouriteBookDto>>
    {
        public Guid UserId { get; set; }
    }
}
