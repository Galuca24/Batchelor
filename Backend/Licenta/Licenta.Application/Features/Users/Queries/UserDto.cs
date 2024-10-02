using Licenta.Application.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries
{
    public class UserDto
    {
        public string? UserId { get; set; }
        public Guid? Id { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Mobile { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public Social? Social { get; set; }

        public UserCloudPhotoDto? UserPhoto { get; set; }
        public List<string>? Roles { get; set; }
    }
}
