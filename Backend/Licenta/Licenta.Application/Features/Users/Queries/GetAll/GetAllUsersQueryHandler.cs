using Licenta.Application.Features.Loans.Queries;
using Licenta.Application.Features.Users.Queries.GetAll;
using Licenta.Application.Features.Users;
using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
{
    private readonly IUserManager _userRepository;
    private readonly IUserPhotoRepository _userPhotoRepository; // Dependency injection pentru UserPhotoRepository

    public GetAllUsersQueryHandler(IUserManager userRepository, IUserPhotoRepository userPhotoRepository)
    {
        _userRepository = userRepository;
        _userPhotoRepository = userPhotoRepository;
    }

    public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();
        var userDtos = new List<UserDto>();

        foreach (var user in users.Value)
        {
            var photoResult = await _userPhotoRepository.GetUserPhotoByUserIdAsync(user.UserId);
            UserCloudPhotoDto userPhotoDto = null;

            if (photoResult.IsSuccess)
            {
                userPhotoDto = new UserCloudPhotoDto
                {
                    UserPhotoId = photoResult.Value.UserPhotoId,
                    PhotoUrl = photoResult.Value.PhotoUrl
                };
            }

            userDtos.Add(new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Bio = user.Bio,
                Mobile = user.Mobile,
                Company = user.Company,
                Location = user.Location,
                Social = user.Social,
                UserPhoto = userPhotoDto, 
                Roles = user.Roles.ToList()
            });
        }

        return new GetAllUsersResponse { Users = userDtos };
    }
}
