using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Commands.ReturnBook
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, ReturnBookCommandResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly IFineRepository _fineRepository;  

        public ReturnBookCommandHandler(IBookRepository bookRepository, ICheckoutRepository checkoutRepository, ILoanRepository loanRepository, IFineRepository fineRepository)
        {
            _bookRepository = bookRepository;
            _checkoutRepository = checkoutRepository;
            _loanRepository = loanRepository;
            _fineRepository = fineRepository;  
        }

        public async Task<ReturnBookCommandResponse> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var bookResult = await _bookRepository.FindByIdAsync(request.BookId);
            if (!bookResult.IsSuccess || bookResult.Value == null)
            {
                return new ReturnBookCommandResponse { Success = false, Message = "Cartea nu a fost găsită." };
            }

            var book = bookResult.Value;
            if (book.BookStatus != BookStatus.Loaned)
            {
                return new ReturnBookCommandResponse { Success = false, Message = "Cartea nu este în stadiul de împrumut." };
            }

            var loanResult = await _loanRepository.GetActiveLoanByBookId(request.BookId);
            if (!loanResult.IsSuccess)
            {
                return new ReturnBookCommandResponse { Success = false, Message = loanResult.Error };
            }

            var loan = loanResult.Value;
            book.BookStatus = BookStatus.Available;
            loan.ReturnDate = DateTime.UtcNow;
            await _bookRepository.UpdateAsync(book);

            var checkout = new Checkout(book.BookId, loan.UserId, loan.LoanDate, loan.DueDate, DateTime.UtcNow, book.Title, book.Author, loan.LoanedBy,loan.DaysLoaned);
            checkout.ReturnDate = DateTime.UtcNow;

            if (checkout.ReturnDate > loan.DueDate)
            {
                checkout.Status = CheckoutStatus.Overdue;
                checkout.DaysOverdue = (checkout.ReturnDate.Value - loan.DueDate).Days;
                var daysLate = (checkout.ReturnDate.Value - loan.DueDate).Days;
                var fineAmount = daysLate * 1;  
                var fine = new Fine(fineAmount, "Intârziere la returnarea cărții",loan.UserId,false,book.BookId,checkout.DaysOverdue,null);
                var fineResult = await _fineRepository.AddAsync(fine);
                if (fineResult.IsSuccess)
                {
                    checkout.Fine = fine.Amount;  
                }
            }
            else
            {
                checkout.Status = CheckoutStatus.Returned;
            }

            var addResult = await _checkoutRepository.AddAsync(checkout);
            if (!addResult.IsSuccess)
            {
                return new ReturnBookCommandResponse { Success = false, Message = "Eroare la înregistrarea returnării cărții." };
            }

            return new ReturnBookCommandResponse
            {
                Success = true,
                Message = "Cartea a fost returnată cu succes.",
                CheckoutId = checkout.CheckoutId
            };
        }
    }
}
    
