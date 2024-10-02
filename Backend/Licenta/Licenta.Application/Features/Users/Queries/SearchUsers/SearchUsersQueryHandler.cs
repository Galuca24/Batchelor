using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Users.Queries.SearchUsers
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, SearchUsersQueryResponse>
    {
        private readonly IUserManager userManager;
        private readonly IUserPhotoRepository userPhotoRepository;

        public SearchUsersQueryHandler(IUserManager userManager, IUserPhotoRepository userPhotoRepository)
        {
            this.userManager = userManager;
            this.userPhotoRepository = userPhotoRepository;
        }

        public async Task<SearchUsersQueryResponse> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var allUsers = await userManager.GetAllAsync();
            if (!allUsers.IsSuccess)
            {
                return new SearchUsersQueryResponse { Success = false, Message = allUsers.Error };
            }

            var users = allUsers.Value.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.Username) && u.Username.ToLower().Contains(request.SearchValue.ToLower())) ||
                    (!string.IsNullOrWhiteSpace(u.Name) && u.Name.ToLower().Contains(request.SearchValue.ToLower()))
                ).ToArray();

            return new SearchUsersQueryResponse
            {
                Success = true,
                Users = users.Select(u => new UserSearchDto
                {
                    UserId = u.UserId,
                    Username = u.Username,
                    Name = u.Name,
                    Email = u.Email,
                    UserPhoto = !string.IsNullOrWhiteSpace(u.UserId) &&
                                userPhotoRepository.GetUserPhotoByUserIdAsync(u.UserId).Result.IsSuccess
                        ? new UserCloudPhotoDto
                        {
                            UserPhotoId = userPhotoRepository.GetUserPhotoByUserIdAsync(u.UserId).Result.Value.UserPhotoId,
                            PhotoUrl = userPhotoRepository.GetUserPhotoByUserIdAsync(u.UserId).Result.Value.PhotoUrl
                        }
                        : null
                }).Take(25).ToArray()
            };
        }
    }
}
