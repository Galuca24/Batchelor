using Licenta.Application.Features.InboxItems.Commands.UpdateInboxItemIsRead;
using Licenta.Application.Persistence;
using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InboxItemRepository : BaseRepository<InboxItem>, IInboxItemRepository
    {
        public InboxItemRepository(LicentaContext context) : base(context)
        {
        }

        public async Task<List<InboxItem>> GetUnreadByUserIdAsync(Guid userId)
        {
            return await _context.InboxItems
                .Where(i => i.UserId == userId && !i.IsRead)
                .ToListAsync();
        }


        public async Task<Result<InboxItemDto>> UpdateIsReadAsync(InboxItem inboxItem, bool isRead)
        {
            inboxItem.UpdateIsRead(isRead);
            await _context.SaveChangesAsync();
            return Result<InboxItemDto>.Success(new InboxItemDto
            {
                UserId = inboxItem.UserId,
                Message = inboxItem.Message,
                CreatedDate = inboxItem.CreatedDate,
                IsRead = inboxItem.IsRead
            });
        }

        public Task<List<InboxItem>> GetByUserIdAsync(Guid userId)
        {
            return Task.FromResult(_context.InboxItems.Where(i => i.UserId == userId).ToList());

        }

    }
    
}
