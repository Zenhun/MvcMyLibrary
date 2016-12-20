using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcMyLibrary.Models
{
    [Table("dbo.Genre")]
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; }
        List<Book> Books { get; set; }
    }
}