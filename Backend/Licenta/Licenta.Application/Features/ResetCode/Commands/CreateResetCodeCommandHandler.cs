using Licenta.Application.Contracts;
using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.ResetCode.Commands
{
    public class CreateResetCodeCommandHandler : IRequestHandler<CreateResetCodeCommand, CreateResetCodeCommandResponse>
    {
        private readonly IPasswordResetCode resetCodeRepository;
        private readonly IEmailService emailService;
        private readonly IUserManager userManager;
        public CreateResetCodeCommandHandler(IPasswordResetCode resetCodeRepository, IUserManager userManager, IEmailService emailService)
        {
            this.resetCodeRepository = resetCodeRepository;
            this.userManager = userManager;
            this.emailService = emailService;
        }

        public async Task<CreateResetCodeCommandResponse> Handle(CreateResetCodeCommand request, CancellationToken cancellationToken)
        {
            var validator = new CreateResetCodeCommandValidator();
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new CreateResetCodeCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            var response = new CreateResetCodeCommandResponse();
            var user = await userManager.FindByEmailAsync(request.Email);
            if (!user.IsSuccess)
            {
                return new CreateResetCodeCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User with the provided email does not exist." }
                };
            }
            await resetCodeRepository.InvalidateExistingCodesAsync(request.Email);

            var resetCode = GenerateRandomCode(8);
            var passwordResetCode = PasswordResetCode.Create(request.Email, resetCode);
            var result = await resetCodeRepository.AddAsync(passwordResetCode.Value);
            if (!result.IsSuccess)
            {
                return new CreateResetCodeCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "Something went wrong while creating the reset code." }
                };
            }
            var emailBody = $"Hi. You recently requested to reset your password. Please you this code {resetCode} to reset your password";
            await emailService.SendEmailAsync(request.Email, "Reset Code", emailBody);
            return new CreateResetCodeCommandResponse
            {
                Success = true,
                Message = "Reset code sent successfully."
            };


        }
        public static string GenerateRandomCode(int length)
        {
            const string availableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[length];
                rng.GetBytes(byteArray);

                var code = new StringBuilder(length);
                foreach (var byteValue in byteArray)
                {
                    code.Append(availableChars[byteValue % availableChars.Length]);
                }

                return code.ToString();
            }
        }

    }
}
