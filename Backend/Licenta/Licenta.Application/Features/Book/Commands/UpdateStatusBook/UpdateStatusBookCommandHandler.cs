using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.UpdateStatusBook
{
    public class UpdateStatusBookCommandHandler : IRequestHandler<UpdateStatusBookCommand, UpdateStatusBookCommandResponse>
    {
        private readonly IBookRepository _bookRepository;

        public UpdateStatusBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        public async Task<UpdateStatusBookCommandResponse> Handle(UpdateStatusBookCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateStatusBookCommandResponse();

            var book = await _bookRepository.FindByIdAsync(request.BookId);

            var validator = new UpdateStatusBookCommandValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new UpdateStatusBookCommandResponse
                {
                    Success = false,
                    Message = validationResult.ToString(),
                    ValidationsErrors = validationResult.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }

            if (book.IsSuccess)
            {
                book.Value.BookStatus = request.Status;
                await _bookRepository.UpdateAsync(book.Value);
                response.Book = new UpdateBookStatusDto
                {
                    BookStatus = book.Value.BookStatus
                };
            }
            else
            {
                response.Success = false;
                response.Message = "Book not found";
            }

            return response;
        }
    }
   
}
