﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Queries.GetAllLoansByUser
{
    public class GetUserLoansQuery : IRequest<List<LoanDto>>
    {
        public Guid UserId { get; set; }
    }
}
