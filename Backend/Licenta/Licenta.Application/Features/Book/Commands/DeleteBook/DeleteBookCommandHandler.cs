using Licenta.Application.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, DeleteBookCommandResponse>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<DeleteBookCommandResponse> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            // Încercăm să găsim cartea înainte de a o șterge
            var book = await _bookRepository.FindByIdAsync(request.BookId);

            if (book == null)
            {
                return new DeleteBookCommandResponse
                {
                    Success = false,
                    Message = "Cartea nu a fost găsită."
                };
            }

            // Procedăm la ștergerea cărții
            var result = await _bookRepository.DeleteAsync(request.BookId);

            if (!result.IsSuccess)
            {
                return new DeleteBookCommandResponse
                {
                    Success = false,
                    Message = result.Error // presupunem că result.Error conține un mesaj de eroare adecvat
                };
            }

            // Dacă totul a decurs bine, returnăm un răspuns de succes
            return new DeleteBookCommandResponse
            {
                Success = true,
                Message = "Cartea a fost ștearsă cu succes."
            };
        }
    }
}
