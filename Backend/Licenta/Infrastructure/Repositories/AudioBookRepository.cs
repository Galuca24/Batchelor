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
    public class AudioBookRepository : BaseRepository<AudioBook>, IAudioBookRepository
    {
        public AudioBookRepository(LicentaContext context) : base(context)
        {
        }

        public async Task<bool> AudioBookExists(Guid id)
        {
            return await _context.AudioBooks.AnyAsync(ab => ab.AudioBookId == id);
        }

        public async Task<Result<List<AudioBook>>> GetAllWithChaptersAsync()
        {
            var audioBooks = await _context.AudioBooks
                .Include(ab => ab.Chapters)
                .ToListAsync();

            return Result<List<AudioBook>>.Success(audioBooks);
        }

        public async Task<AudioBook> GetAudioBookWithChaptersAsync(Guid id)
        {
            return await _context.AudioBooks
                .Include(ab => ab.Chapters)
                .FirstOrDefaultAsync(ab => ab.AudioBookId == id);
        }
    }
    
}
