using Licenta.Domain.Entities;


namespace Licenta.Application.Persistence
{
    public interface IPasswordResetCode : IAsyncRepository<PasswordResetCode>
    {
        Task<bool> HasValidCodeByEmailAsync(string email, string code);
        Task InvalidateExistingCodesAsync(string email);
    }
}
