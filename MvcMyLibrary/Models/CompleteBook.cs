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
                MyLibraryContext dbLibrary = new MyLibraryContext();

                genreList = (from g in dbLibrary.Genres
                             orderby g.GenreName
                             select g).ToList();


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
            MyLibraryContext dbLibrary = new MyLibraryContext();

            var books2 = (from b in dbLibrary.Books
                          join a in dbLibrary.Authors on b.AuthorId equals a.AuthorId
                          join g in dbLibrary.Genres on b.GenreId equals g.GenreId
                          orderby b.Title
                          select new { b.BookId, b.Title, a.Name, a.Surname, g.GenreId, g.GenreName, b.ImageUrl }).ToList();

            if (GenreId != -1)
            {
                books2 = (from b in dbLibrary.Books
                          join a in dbLibrary.Authors on b.AuthorId equals a.AuthorId
                          join g in dbLibrary.Genres on b.GenreId equals g.GenreId
                          where g.GenreId == GenreId
                          orderby b.Title
                          select new { b.BookId, b.Title, a.Name, a.Surname, g.GenreId, g.GenreName, b.ImageUrl }).ToList();
            }


            //I made a class with all the elements of one book, gather from 3 tables: Books, Authors and Genres
            List<CompleteBook> books = new List<CompleteBook>();

            foreach (var book in books2)
            {
                var modelBook = new CompleteBook();
                modelBook.BookId = book.BookId;
                modelBook.Title = book.Title;
                modelBook.AuthorName = book.Name;
                modelBook.AuthorSurname = book.Surname;
                modelBook.GenreId = book.GenreId;
                modelBook.Genre = book.GenreName;
                modelBook.CoverImageUrl = book.ImageUrl;

                books.Add(modelBook);
            }
            
            return books;
        }
    }
}