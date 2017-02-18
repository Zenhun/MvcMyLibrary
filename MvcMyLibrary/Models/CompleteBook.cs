using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MvcMyLibrary.Models
{
    public class CompleteBook
    {
        public int BookId { get; set; }
        [Required(ErrorMessage="Please enter book title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter author's name")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "Please enter author's surname")]
        public string AuthorSurname { get; set; }
        [Required(ErrorMessage = "Please select genre")]
        public int GenreId { get; set; }
        [Required(ErrorMessage = "Please enter genre")]
        public string Genre { get; set; }
        public string CoverImageUrl { get; set; }
        IEnumerable<Genre> genreList;
        public IEnumerable<Genre> GenreList
        {
            get
            {
                genreList = BookActions.GetGenres();
                return genreList;
            }
            set { genreList = value; }
        }

        public List<CompleteBook> GetCompleteBooks()
        {
            return GetCompleteBooks(-1);
        }

        public List<CompleteBook> GetCompleteBooks(int GenreId)
        {
            var books = BookActions.GetCompleteBooks(GenreId);
            
            return books;
        }
    }
}