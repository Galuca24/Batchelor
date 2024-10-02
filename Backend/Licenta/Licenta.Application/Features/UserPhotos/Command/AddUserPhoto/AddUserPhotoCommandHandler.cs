using Licenta.Application.Features.Users;
using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Command.AddUserPhoto
{
    public class AddUserPhotoCommandHandler : IRequestHandler<AddUserPhotoCommand, AddUserPhotoCommandResponse>
    {
        private readonly IUserPhotoRepository userPhotoRepository;

        public AddUserPhotoCommandHandler(IUserPhotoRepository userPhotoRepository)
        {
            this.userPhotoRepository = userPhotoRepository;
        }

        public Task<AddUserPhotoCommandResponse> Handle(AddUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var validator = new AddUserPhotoCommandValidator();
            var validatorResult = validator.Validate(request);
            if (!validatorResult.IsValid)
            {
                return Task.FromResult(new AddUserPhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var userPhoto = UserPhoto.Create(request.PhotoUrl, request.UserId);
            if (!userPhoto.IsSuccess)
            {
                return Task.FromResult(new AddUserPhotoCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { userPhoto.Error }
                });
            }

            userPhotoRepository.AddAsync(userPhoto.Value);
            return Task.FromResult(new AddUserPhotoCommandResponse
            {
                Success = true,
                UserPhoto = new UserCloudPhotoDto
                {
                    UserPhotoId = userPhoto.Value.UserPhotoId,
                    PhotoUrl = userPhoto.Value.PhotoUrl
                }
            });
        }
    }
}
