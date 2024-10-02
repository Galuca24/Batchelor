using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.SearchBooks
{
    public class SearchBooksQuery : IRequest<SearchBooksQueryResponse>
    {
        public string SearchValue { get; set; }
    }
    
}
