using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Queries.GetUserPhoto
{
    public class GetUserPhotoQuery : IRequest<GetUserPhotoQueryResponse>
    {
        public string UserId { get; set; }
    }
}
