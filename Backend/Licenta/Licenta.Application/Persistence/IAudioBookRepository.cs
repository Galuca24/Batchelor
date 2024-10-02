using Licenta.Domain.Common;
using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Persistence
{
    public interface IAudioBookRepository : IAsyncRepository<AudioBook>
    {
        Task<Result<List<AudioBook>>> GetAllWithChaptersAsync();
        Task<AudioBook> GetAudioBookWithChaptersAsync(Guid id);

        Task<bool> AudioBookExists(Guid id);

    }
}
