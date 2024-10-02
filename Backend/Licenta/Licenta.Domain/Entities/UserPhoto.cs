using Licenta.Domain.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Domain.Entities
{
    public class UserPhoto
    {
        private UserPhoto(string photoUrl, string userId)
        {
            UserPhotoId = Guid.NewGuid();
            PhotoUrl = photoUrl;
            UserId = userId;
        }
        public UserPhoto()
        {

        }
        public Guid UserPhotoId { get; private set; }
        public string PhotoUrl { get; private set; }
        public string UserId { get; private set; }
        public static Result<UserPhoto> Create(string photoUrl, string userId)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                return Result<UserPhoto>.Failure("Photo url is required.");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<UserPhoto>.Failure("User id is required.");
            }
            return Result<UserPhoto>.Success(new UserPhoto(photoUrl, userId));
        }

        public Result<UserPhoto> UpdatePhoto(string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(photoUrl))
            {
                return Result<UserPhoto>.Failure("Photo url is required.");
            }
            PhotoUrl = photoUrl;
            return Result<UserPhoto>.Success(this);
        }
    }
}
