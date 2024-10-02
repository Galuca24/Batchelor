using Licenta.Application.Features.Loans.Queries;
using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommandResponse : BaseResponse
    {
        public UpdateUserCommandResponse() : base()
        {
        }

        public UpdateUserDto User { get; set; }
    }
}

