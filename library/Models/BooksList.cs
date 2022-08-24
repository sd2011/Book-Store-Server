using System;
using System.Collections.Generic;

namespace library.Models
{
    public partial class BooksList
    {
      

        public int BookId { get; set; }
        public string? Genre { get; set; }
        public string? PublishedBookDate { get; set; }
        public int? NumOfCopies { get; set; }
        public string? Author { get; set; }
        public string? Pages { get; set; }
        public string? Title { get; set; }
        public string? BookLanguage { get; set; }
        public string? ImageLink { get; set; }
        public string? Country { get; set; }

    }
}
