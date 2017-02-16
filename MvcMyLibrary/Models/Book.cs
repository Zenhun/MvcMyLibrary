using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMyLibrary.Models
{
    [Table("dbo.Book")]
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        //Foreign Key
        public int AuthorId { get; set; }
        //Foreign Key
        public int GenreId { get; set; }
        public string ImageUrl { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Authors { get; set; }
        [ForeignKey("GenreId")]
        public virtual Genre Genres { get; set; }
    }
}