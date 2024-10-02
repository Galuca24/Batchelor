using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Command.UpdateUserPhoto
{
    public class UpdateUserPhotoCommand : IRequest<UpdateUserPhotoCommandResponse>
    {
        public string UserPhotoId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
