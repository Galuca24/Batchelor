using MediatR;

namespace Licenta.Application.Features.Loans.Commands.DeleteUser
{
    public class DeleteUserCommand: IRequest<DeleteBookCommandResponse>
    {
        public Guid UserId { get; set; }
    }
}
