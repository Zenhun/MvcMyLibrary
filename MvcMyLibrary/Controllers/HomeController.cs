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
        CompleteBook bookList = new CompleteBook();

        public ActionResult Index()
        {
            List<CompleteBook> books = bookList.GetCompleteBooks();
            
            return View(books);
        }

        public ActionResult IndexGenre(int GenreId)
        {
            List<CompleteBook> books = bookList.GetCompleteBooks(GenreId);

            return View("Index", books);
        }

        [ChildActionOnly]
        public ActionResult SideBar()
        {
            List<Genre> genres = (from g in dbLibrary.Genres
                                  orderby g.GenreName
                                  select g).ToList();

            //obrati pažnju da se vraća PartialView
            return PartialView(genres);
        }

        public ActionResult SaveBook(string Title, string AuthorName, string AuthorSurname, int GenreId, string ImageUrl)
        {
            //Debug Output just to test user input
            System.Diagnostics.Debug.WriteLine("Title is " + Title);
            System.Diagnostics.Debug.WriteLine("Author is " + AuthorName + " " + AuthorSurname);

            BookActions.BookSave(Title, AuthorName, AuthorSurname, GenreId, ImageUrl);
            
            return RedirectToAction("IndexGenre", new { GenreId });
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