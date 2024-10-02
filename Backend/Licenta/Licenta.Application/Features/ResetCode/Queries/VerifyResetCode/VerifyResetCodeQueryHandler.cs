using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.ResetCode.Queries.VerifyResetCode
{
    public class VerifyResetCodeQueryHandler : IRequestHandler<VerifyResetCodeQuery, VerifyResetCodeResponse>
    {
        private readonly IPasswordResetCode passwordResetCodeRepository;
        public VerifyResetCodeQueryHandler(IPasswordResetCode passwordResetCodeRepository)
        {
            this.passwordResetCodeRepository = passwordResetCodeRepository;
        }

        public async Task<VerifyResetCodeResponse> Handle(VerifyResetCodeQuery request, CancellationToken cancellationToken)
        {

            var hasValidCode = await passwordResetCodeRepository.HasValidCodeByEmailAsync(request.Email, request.Code);
            if (!hasValidCode)
            {
                return new VerifyResetCodeResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Invalid reset code or expired." }
                };

            }
            //await passwordResetCodeRepository.InvalidateExistingCodesAsync(request.Email);
            return new VerifyResetCodeResponse
            {
                Success = true
            };

        }
    }
}
