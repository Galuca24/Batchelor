using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Mobile { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public string? Facebook { get; set; }
        public string? TwitterX { get; set; }
        public string? LinkedIn { get; set; }
        public string? Instagram { get; set; }
        public string? GitHub { get; set; }

        public void UpdateData(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
