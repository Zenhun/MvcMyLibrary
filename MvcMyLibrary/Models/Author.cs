using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMyLibrary.Models
{
    [Table("dbo.Author")]
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Book> Books { get; set; }
    }
}