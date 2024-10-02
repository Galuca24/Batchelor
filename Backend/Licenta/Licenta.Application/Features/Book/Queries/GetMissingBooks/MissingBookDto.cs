﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book.Queries.GetMissingBooks
{
    public class MissingBookDto
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
    }

}
