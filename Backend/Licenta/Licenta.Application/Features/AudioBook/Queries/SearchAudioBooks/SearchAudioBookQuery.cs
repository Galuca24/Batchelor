using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.AudioBook.Queries.SearchAudioBooks
{
    public class SearchAudioBookQuery : IRequest<SearchAudioBookQueryResponse>
    {
        public string SearchValue { get; set; }
    }
    
}
