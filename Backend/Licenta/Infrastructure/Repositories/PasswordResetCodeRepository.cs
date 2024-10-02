using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PasswordResetCodeRepository : BaseRepository<PasswordResetCode>, IPasswordResetCode
    {
        public PasswordResetCodeRepository(LicentaContext context) : base(context)
        {

        }

        public async Task<bool> HasValidCodeByEmailAsync(string email, string code)
        {

            return await _context.PasswordResetCodes
                .AnyAsync(c => c.Email == email && c.Code == code && c.ExpirationTime > DateTime.UtcNow);
        }
        public async Task InvalidateExistingCodesAsync(string email)
        {
            var codesToInvalidate = await _context.PasswordResetCodes
                .Where(code => code.Email == email && code.ExpirationTime > DateTime.UtcNow)
                .ToListAsync();

            foreach (var code in codesToInvalidate)
            {
                code.Invalidate();
            }

            await _context.SaveChangesAsync();
        }
    }
}
