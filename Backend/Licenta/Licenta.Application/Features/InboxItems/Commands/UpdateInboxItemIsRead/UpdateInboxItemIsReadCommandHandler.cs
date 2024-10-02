using Licenta.Application.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead
{
    public class UpdateInboxItemIsReadCommandHandler : IRequestHandler<UpdateInboxItemIsReadCommand, UpdateInboxItemIsReadCommandResponse>
    {
        private readonly IInboxItemRepository inboxItemRepository;

        public UpdateInboxItemIsReadCommandHandler(IInboxItemRepository inboxItemRepository)
        {
            this.inboxItemRepository = inboxItemRepository;
        }
        public async Task<UpdateInboxItemIsReadCommandResponse> Handle(UpdateInboxItemIsReadCommand request, CancellationToken cancellationToken)
        {
            var inboxItem = await inboxItemRepository.FindByIdAsync(request.InboxItemId);
            if (!inboxItem.IsSuccess)
            {
                return new UpdateInboxItemIsReadCommandResponse
                {
                    Success = false,
                    Message = inboxItem.Error
                };
            }
            var result = await inboxItemRepository.UpdateIsReadAsync(inboxItem.Value, true);
            if (!result.IsSuccess)
            {
                return new UpdateInboxItemIsReadCommandResponse
                {
                    Success = false,
                    Message = result.Error
                };
            }
            return new UpdateInboxItemIsReadCommandResponse
            {
                Success = true
            };

        }

    }
}
