using Licenta.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Licenta.Application.Features.InboxItems.Queries.GetByUserId;
using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Queries.GetUnreaByUserId
{
    public class GetUnreadByUserIdQueryHandler : IRequestHandler<GetUnreadByUserIdQuery, GetUnreadByUserIdQueryResponse>
    {
        private readonly IInboxItemRepository _inboxItemRepository;
        private readonly IUserRepository _userRepository;

        public GetUnreadByUserIdQueryHandler(IInboxItemRepository inboxItemRepository, IUserRepository userRepository)
        {
            _inboxItemRepository = inboxItemRepository;
            _userRepository = userRepository;
        }

        public async Task<GetUnreadByUserIdQueryResponse> Handle(GetUnreadByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userExists = await _userRepository.FindByIdAsync(request.UserId);
            if (!userExists.IsSuccess)
            {
                return new GetUnreadByUserIdQueryResponse
                {
                    Success = false,
                    ValidationsErrors = new List<string> { "User with the provided ID does not exist." }
                };


            }
            var inboxItems = await _inboxItemRepository.GetUnreadByUserIdAsync(request.UserId);
            return new GetUnreadByUserIdQueryResponse
            {
                Success = true,
                InboxItems = inboxItems.Select(x => new GetByUserIdDto
                {
                    InboxItemId = x.InboxItemId,
                    UserId = x.UserId,
                    Message = x.Message,
                    CreatedDate = x.CreatedDate,
                    IsRead = x.IsRead,

                }).ToList()
            };

        }
    }
}
