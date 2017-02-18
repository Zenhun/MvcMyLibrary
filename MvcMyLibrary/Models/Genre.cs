using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMyLibrary.Models
{
    //table with the exact name as in database
    [Table("dbo.Genre")]
    public class Genre
    {
        [Required]
        public int GenreId { get; set; }
        [Required]
        public string GenreName { get; set; }
        List<Book> Books { get; set; }

        public static List<Genre> GetGenres()
        {
            return BookActions.GetGenres();
        }
    }
}