﻿using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.UserPhotos.Queries.GetUserPhoto
{
    public class GetUserPhotoQueryHandler : IRequestHandler<GetUserPhotoQuery, GetUserPhotoQueryResponse>
    {
        private readonly IUserPhotoRepository userPhotoRepository;

        public GetUserPhotoQueryHandler(IUserPhotoRepository userPhotoRepository)
        {
            this.userPhotoRepository = userPhotoRepository;
        }

        public async Task<GetUserPhotoQueryResponse> Handle(GetUserPhotoQuery request, CancellationToken cancellationToken)
        {
            var userPhoto = await userPhotoRepository.GetUserPhotoByUserIdAsync(request.UserId);
            if (!userPhoto.IsSuccess)
            {
                return new GetUserPhotoQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { userPhoto.Error }
                };
            }

            return new GetUserPhotoQueryResponse
            {
                Success = true,
                UserPhotoId = userPhoto.Value.UserPhotoId,
                UserPhotoUrl = userPhoto.Value.PhotoUrl
            };

        }

    }
}
