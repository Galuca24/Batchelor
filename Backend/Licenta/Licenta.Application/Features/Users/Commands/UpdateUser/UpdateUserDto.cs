using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserDto
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
        public UserCloudPhotoDto? UserPhoto { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Mobile { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public Social? Social { get; set; }
    }
}
