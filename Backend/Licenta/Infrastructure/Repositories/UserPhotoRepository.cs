using Licenta.Application.Persistence;
using Licenta.Domain.Common;
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
    public class UserPhotoRepository : BaseRepository<UserPhoto>, IUserPhotoRepository
    {
        public UserPhotoRepository(LicentaContext context) : base(context)
        {
        }

        public async Task<Result<UserPhoto>> GetUserPhotoByUserIdAsync(string userId)
        {
            var userPhoto = await _context.UserPhotos
                                .Where(up => up.UserId == userId)
                                .FirstOrDefaultAsync();

            if (userPhoto == null)
            {
                return Result<UserPhoto>.Failure("User photo not found");
            }

            return Result<UserPhoto>.Success(userPhoto);

        }
    }
}
