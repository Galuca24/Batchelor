﻿using Licenta.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Licenta.Application.Features.Book
{
    public class BookDto
    {
        public Guid BookId { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        public BookStatus BookStatus { get; set; }
        public string? ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
    }
}
