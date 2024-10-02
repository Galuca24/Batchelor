using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;


namespace Licenta.Application.Features.Book.Commands.CreateBook
{
    public class CreateBookCommandHandler: IRequestHandler<CreateBookCommand, CreateBookCommandResponse>
    {
        private readonly IBookRepository _bookRepository;

        public CreateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<CreateBookCommandResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var validator=new CreateBookCommandValidator();
            var validatorResult = await validator.ValidateAsync(request,cancellationToken);
            if (!validatorResult.IsValid)
            {
                return new CreateBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = validatorResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }
            

            var book = Domain.Entities.Book.Create(request.Title, request.Author, request.Genre, request.Description, request.ISBN, request.BookStatus,request.ImageUrl, request.NumberOfCopies);
            if(!book.IsSuccess)
            {
                return new CreateBookCommandResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { book.Error}
                };
            }
            await _bookRepository.AddAsync(book.Value);
            return new CreateBookCommandResponse
            {
                Success = true,
                Book=new CreateBookDto
                {
                    BookId = book.Value.BookId,
                    Title = book.Value.Title,
                    Author = book.Value.Author,
                    Genre = book.Value.Genre,
                    Description = book.Value.Description,
                    ISBN = book.Value.ISBN,
                    BookStatus = book.Value.BookStatus
                }
            };
        }
       

    }
}
