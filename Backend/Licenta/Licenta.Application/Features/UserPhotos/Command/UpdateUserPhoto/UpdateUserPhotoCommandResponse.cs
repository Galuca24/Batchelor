using Licenta.Application.Features.Users;
using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Command.UpdateUserPhoto
{
    public class UpdateUserPhotoCommandResponse : BaseResponse
    {
        public UpdateUserPhotoCommandResponse() : base()
        {
        }

        public UserCloudPhotoDto UserPhoto { get; set; }
    }
}
