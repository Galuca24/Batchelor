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
    public class ChapterRepository : BaseRepository<Chapter>, IChapterRepository
    {
        public ChapterRepository(LicentaContext context) : base(context)
        {
        }

        public async Task<Result<List<Chapter>>> GetChaptersByAudioBookIdAsync(Guid audioBookId)
        {
            try
            {
                var chapters = await _context.Chapters
                    .Where(c => c.AudioBookId == audioBookId)
                    .ToListAsync();

                return Result<List<Chapter>>.Success(chapters);
            }
            catch (Exception ex)
            {
                return Result<List<Chapter>>.Failure($"An error occurred while retrieving chapters: {ex.Message}");
            }
        }

    }
    
}
