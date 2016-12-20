using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMyLibrary.Models
{
    public class CompleteBook
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorSurname { get; set; }
        public string Genre { get; set; }
        public string CoverImageUrl { get; set; }
    }
}