using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Queries.GetUserPhoto
{
    public class GetUserPhotoQueryResponse : BaseResponse
    {
        public GetUserPhotoQueryResponse() : base()
        {
        }
        public Guid UserPhotoId { get; set; }
        public string UserPhotoUrl { get; set; }
    }
}
