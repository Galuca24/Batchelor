using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Queries.SearchAudioBooks
{
    public class SearchAudioBookQueryResponse : BaseResponse
    {
        public SearchAudioBookDto[] AudioBooks { get; set; } = [];
    }
}
