using Licenta.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface IInboxItemRepository : IAsyncRepository<InboxItem>
    {
        Task<Result<InboxItemDto>> UpdateIsReadAsync(InboxItem inboxItem, bool isRead);

        Task<List<InboxItem>> GetByUserIdAsync(Guid userId);

        Task<List<InboxItem>> GetUnreadByUserIdAsync(Guid userId);

    }
}
