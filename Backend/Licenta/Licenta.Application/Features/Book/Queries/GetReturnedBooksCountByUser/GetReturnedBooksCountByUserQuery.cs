using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetReturnedBooksCountByUser
{
    public class GetReturnedBooksCountByUserQuery : IRequest<int>
    {
        public Guid UserId { get; set; }

        public GetReturnedBooksCountByUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
