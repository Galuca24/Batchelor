using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Loans.Commands.LoanBookCommand
{
    public class LoanBookCommandHandler : IRequestHandler<LoanBookCommand, LoanBookCommandResponse>
    {
        private readonly IBookRepository bookRepository;
        private readonly ILoanRepository loanRepository;
        private readonly IUserManager userManager;
        private readonly IFineRepository fineRepository;

        public LoanBookCommandHandler(IBookRepository bookRepository, ILoanRepository loanRepository, IUserManager userManager, IFineRepository fineRepository)
        {
            this.bookRepository = bookRepository;
            this.loanRepository = loanRepository;
            this.userManager = userManager;
            this.fineRepository = fineRepository;
        }

        public async Task<LoanBookCommandResponse> Handle(LoanBookCommand request, CancellationToken cancellationToken)
        {
            var bookResult = await bookRepository.FindByIdAsync(request.BookId);

            if (!bookResult.IsSuccess || bookResult.Value.BookStatus != BookStatus.Available)
            {
                return new LoanBookCommandResponse { Success = false, Message = "Cartea nu este disponibilă." };
            }

            var userExists = await userManager.FindByIdAsync(request.UserId);
            if(!userExists.IsSuccess )
            {
                return new LoanBookCommandResponse { Success = false, Message = "Utilizatorul nu există." };
            }

            if (await loanRepository.CountActiveLoansByUserIdAsync(request.UserId) >= 2)
            {
                return new LoanBookCommandResponse { Success = false, Message = "Utilizatorul a atins limita de împrumuturi." };
            }

            if (await fineRepository.HasUnpaidFinesAsync(request.UserId))
            {
                return new LoanBookCommandResponse { Success = false, Message = "Utilizatorul are amenzile neplătite." };
            }


            var book = bookResult.Value;

            var loan = new Loan
            {
                LoanId = Guid.NewGuid(),
                BookId = request.BookId,
                UserId = request.UserId,
                LoanDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddMinutes(40), 
                LoanedBy= userExists.Value.Username
            };

            await loanRepository.AddAsync(loan);

            book.BookStatus = BookStatus.Loaned;
            await bookRepository.UpdateAsync(book);

            return new LoanBookCommandResponse { Success = true, Message = "Împrumut realizat cu succes." };
        }

    }
}
