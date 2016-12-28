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
                          select new { b.BookId, b.Title, a.Name, a.Surname, g.GenreName, b.ImageUrl }).ToList();

            if (GenreId != -1)
            {
                books2 = (from b in dbLibrary.Books
                          join a in dbLibrary.Authors on b.AuthorId equals a.AuthorId
                          join g in dbLibrary.Genres on b.GenreId equals g.GenreId
                          where g.GenreId == GenreId
                          select new { b.BookId, b.Title, a.Name, a.Surname, g.GenreName, b.ImageUrl }).ToList();
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
                modelBook.Genre = book.GenreName;
                modelBook.CoverImageUrl = book.ImageUrl;

                books.Add(modelBook);
            }

            return books;
        }
    }
}