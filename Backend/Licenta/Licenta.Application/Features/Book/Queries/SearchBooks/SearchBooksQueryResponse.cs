using Licenta.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.SearchBooks
{
    public class SearchBooksQueryResponse : BaseResponse
    {
        public SearchBookDto[] Books { get; set; } = [];
    }
}
