using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Commands.RemoveBookFromFavourite
{
    public  class RemoveBookFromFavouriteCommandHandler : IRequestHandler<RemoveBookFromFavouriteCommand, RemoveBookFromFavouriteCommandResponse>
    {

        private readonly IFavouriteRepository _favouriteRepository;

        public RemoveBookFromFavouriteCommandHandler(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }


        public async Task<RemoveBookFromFavouriteCommandResponse> Handle(RemoveBookFromFavouriteCommand request, CancellationToken cancellationToken)
        {
            var result = await _favouriteRepository.DeleteByUserIdAndBookIdAsync(request.UserId, request.BookId);
            if (!result.IsSuccess)
            {
                return new RemoveBookFromFavouriteCommandResponse { Success = false, Message = result.Error };
            }

            return new RemoveBookFromFavouriteCommandResponse { Success = true, Message = "Favorite removed successfully." };
        }

    }

}
