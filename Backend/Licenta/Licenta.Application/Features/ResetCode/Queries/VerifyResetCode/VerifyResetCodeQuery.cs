﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.ResetCode.Queries.VerifyResetCode
{
    public class VerifyResetCodeQuery : IRequest<VerifyResetCodeResponse>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
