using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Favourites.Commands.AddFavourite
{
    public class AddFavouriteCommandHandler : IRequestHandler<AddFavouriteCommand, AddFavouriteResponse>
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public AddFavouriteCommandHandler(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }

        public async Task<AddFavouriteResponse> Handle(AddFavouriteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Verifică dacă cartea este deja favorită pentru utilizatorul dat
                var existingFavourite = await _favouriteRepository.FindByUserIdAndBookIdAsync(request.UserId, request.BookId);
                if (existingFavourite != null)
                {
                    return new AddFavouriteResponse { Success = false, Message = "Book already added as favorite." };
                }

                // Dacă nu există, adaugă cartea în favorite
                var favourite = new Favourite
                {
                    UserId = request.UserId,
                    BookId = request.BookId,
                    AddedOn = DateTime.UtcNow
                };

                await _favouriteRepository.AddAsync(favourite);
                return new AddFavouriteResponse { Success = true, Message = "Favorite added successfully." };
            }
            catch (Exception ex)
            {
                return new AddFavouriteResponse { Success = false, Message = $"Error adding favorite: {ex.Message}" };
            }
        }
    }
}
