using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcMyLibrary.Models;

namespace MvcMyLibrary.Controllers
{
    public class HomeController : Controller
    {
        MyLibraryContext dbLibrary = new MyLibraryContext();
        public ActionResult Index()
        {
            //var books = dbLibrary.Books.ToList();
            var books2 = (from b in dbLibrary.Books 
                            join a in dbLibrary.Authors on b.AuthorId equals a.AuthorId 
                            join g in dbLibrary.Genres on b.GenreId equals g.GenreId 
                            select new {b.BookId,b.Title,a.Name,a.Surname, g.GenreName,b.ImageUrl}).ToList();
            
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

            //SELECT BookId,Title,a.Name,a.Surname,g.Name AS 'Genre',ImageUrl 
            //FROM Book b 
            //JOIN Author a ON b.AuthorID = a.AuthorID 
            //JOIN Genre g ON b.GenreId = g.GenreId

            return View(books);
        }

        public ActionResult IndexGenre(int GenreId)
        {
            var books2 = (from b in dbLibrary.Books
                          join a in dbLibrary.Authors on b.AuthorId equals a.AuthorId
                          join g in dbLibrary.Genres on b.GenreId equals g.GenreId
                          where g.GenreId == GenreId
                          select new { b.BookId, b.Title, a.Name, a.Surname, g.GenreName, b.ImageUrl }).ToList();
            
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

            return View("Index", books);
        }

        [ChildActionOnly]
        public ActionResult SideBar()
        {
            List<Genre> genres = (from g in dbLibrary.Genres
                         select g).ToList();

            return PartialView(genres);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}