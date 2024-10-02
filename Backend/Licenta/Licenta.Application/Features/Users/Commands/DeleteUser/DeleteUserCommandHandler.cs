using MediatR;
using Licenta.Application.Persistence;
using Licenta.Application.Features.Loans.Commands.DeleteUser;
namespace Licenta.Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler: IRequestHandler<DeleteUserCommand, DeleteBookCommandResponse>
    {
        private readonly IUserManager userRepository;
        private readonly IUserRepository userRepository1;

        public DeleteUserCommandHandler(IUserManager userRepository, IUserRepository userRepository1)
        {
            this.userRepository = userRepository;
            this.userRepository1 = userRepository1;
        }

        public async Task<DeleteBookCommandResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await userRepository.DeleteAsync(request.UserId);
            var result1 = await userRepository1.DeleteAsync(request.UserId);
            if (!result.IsSuccess && !result1.IsSuccess)
                return new DeleteBookCommandResponse { Success = false, Message = result.Error };
            return new DeleteBookCommandResponse { Success = true };
        }
    }
}
