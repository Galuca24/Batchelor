using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Queries
{
    public class UserSearchDto
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserCloudPhotoDto? UserPhoto { get; set; }
    }
}
