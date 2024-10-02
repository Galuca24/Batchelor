using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Command.AddUserPhoto
{
    public class AddUserPhotoCommand : IRequest<AddUserPhotoCommandResponse>
    {
        public IFormFile File { get; set; }
        public string UserId { get; set; }
        public string PhotoUrl { get; set; }
    }
}
